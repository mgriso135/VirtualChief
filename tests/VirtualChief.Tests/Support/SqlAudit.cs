using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace VirtualChief.Tests.Support;

/// <summary>
/// Static analysis of the C# sources: extracts SQL strings and inventories the
/// MySQL-isms that must be dealt with when migrating to PostgreSQL / .NET Core.
/// </summary>
public static class SqlAudit
{
    static readonly Regex CsStringLiteral =
        new("\"(?:[^\"\\\\]|\\\\.)*\"|@\"(?:[^\"]|\"\")*\"", RegexOptions.Compiled);

    // A SQL string must contain one of these to be considered SQL.
    static readonly string[] SqlKick = { "SELECT ", "INSERT ", "UPDATE ", "DELETE ", "CREATE ",
        "ALTER ", "DROP ", "TRUNCATE ", " FROM ", " INTO ", " JOIN " };

    static readonly Regex TableRef =
        new(@"(?:FROM|JOIN|INTO|UPDATE|TABLE|TRUNCATE|DELETE\s+FROM)\s+([A-Za-z0-9_\.`]+)", RegexOptions.IgnoreCase | RegexOptions.Compiled);

    // Tokens that are standard SQL keywords (never flagged as reserved-word identifiers).
    static readonly HashSet<string> SqlKeywords = new(StringComparer.OrdinalIgnoreCase)
    {
        "SELECT","FROM","WHERE","AND","OR","NOT","NULL","IS","IN","AS","ON","JOIN","INNER","LEFT",
        "RIGHT","FULL","OUTER","CROSS","GROUP","BY","ORDER","ASC","DESC","HAVING","LIMIT","OFFSET",
        "SET","VALUES","INSERT","INTO","UPDATE","DELETE","CASE","WHEN","THEN","ELSE","END","LIKE",
        "BETWEEN","EXISTS","COUNT","SUM","AVG","MIN","MAX","DISTINCT","UNION","ALL","ANY","EXCEPT",
        "INTERSECT","OVERLAPS","BOTH","LEADING","TRAILING","FETCH","FOR","GRANT","TO","USING","WITH",
        "RETURNING","PRIMARY","KEY","FOREIGN","REFERENCES","CHECK","CONSTRAINT","DEFAULT","UNIQUE",
        "CREATE","ALTER","DROP","TRUNCATE","TABLE","VIEW","INDEX","DATABASE","SEQUENCE","TRIGGER",
        "CAST","COALESCE","IFNULL","CONCAT","SUBSTRING","LENGTH","ROUND","DATE","TIME","TIMESTAMP",
        "CURRENT_DATE","CURRENT_TIME","CURRENT_TIMESTAMP","NOW","START","TRANSACTION","COMMIT",
        "ROLLBACK","IF","REPLACE","HAVING","DISTINCTROW","HIGH_PRIORITY","LOW_PRIORITY","STRAIGHT_JOIN",
        "CONVERT","FROM_UNIXTIME","UNIX_TIMESTAMP","GROUP_CONCAT","DATE_FORMAT","DATE_ADD","CURDATE"
    };

    // Postgres reserved / reserved-keywords that break when used as a bare identifier.
    // (subset relevant for an audit; "user" is reserved in Postgres, not in MySQL)
    public static readonly HashSet<string> PostgresReservedWords = new(StringComparer.OrdinalIgnoreCase)
    {
        "user","groups","group","order","between","when","where","limit","current_user",
        "current_date","current_time","current_timestamp","interval","collate","constraint",
        "default","freeze","full","grant","outer","overlaps","placing","references","session_user",
        "similar","some","symmetric","trailing","update","vacuum","verbose","leading","check",
        "fetch","ilike","isnull","notnull","offset","analyze"
    };

    // MySQL-only constructs that need a dialect pass on PostgreSQL.
    public static readonly Dictionary<string, string> MysqlOnlyPatterns = new()
    {
        ["backtick"] = @"`[A-Za-z0-9_]+`",
        ["DATE_FORMAT"] = @"DATE_FORMAT\s*\(",
        ["IFNULL"] = @"IFNULL\s*\(",
        ["NOW()"] = @"\bNOW\s*\(\s*\)",
        ["CURDATE"] = @"\bCURDATE\s*\(",
        ["GROUP_CONCAT"] = @"GROUP_CONCAT\s*\(",
        ["ON DUPLICATE KEY"] = @"ON\s+DUPLICATE\s+KEY",
        ["LAST_INSERT_ID"] = @"LAST_INSERT_ID\s*\(",
        ["FOUND_ROWS"] = @"FOUND_ROWS\s*\(",
        ["UNIX_TIMESTAMP"] = @"UNIX_TIMESTAMP\s*\(",
        ["FROM_UNIXTIME"] = @"FROM_UNIXTIME\s*\(",
        ["SUBSTRING_INDEX"] = @"SUBSTRING_INDEX\s*\(",
        ["DATE_ADD"] = @"DATE_ADD\s*\(",
    };

    public sealed record SqlOccurrence(string File, string Sql);

    public static IEnumerable<SqlOccurrence> ExtractSql()
    {
        foreach (var file in AllCsFiles())
        {
            var text = File.ReadAllText(file);
            foreach (Match m in CsStringLiteral.Matches(text))
            {
                var lit = m.Value;
                if (lit.StartsWith('@')) continue; // verbatim strings: rare in this codebase
                var body = lit.Substring(1, lit.Length - 2);
                if (SqlKick.Any(k => body.ToUpperInvariant().Contains(k)))
                    yield return new SqlOccurrence(Rel(file), body);
            }
        }
    }

    public static IEnumerable<string> AllCsFiles()
    {
        var root = RepoPaths.Root;
        foreach (var file in Directory.EnumerateFiles(root, "*.cs", SearchOption.AllDirectories))
        {
            var p = file.Replace('\\', '/');
            if (p.Contains("/obj/") || p.Contains("/bin/") || p.Contains("/packages/")
                || p.Contains("/.git/") || p.Contains("/tests/") || p.Contains("/.vs/"))
                continue;
            yield return file;
        }
    }

    public static string Rel(string path) =>
        Path.GetRelativePath(RepoPaths.Root, path).Replace('\\', '/');

    /// <summary>Distinct table names referenced in C# SQL, with hit counts.</summary>
    public static Dictionary<string, int> TableReferences()
    {
        var counts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        foreach (var occ in ExtractSql())
        {
            foreach (Match m in TableRef.Matches(occ.Sql))
            {
                var name = m.Groups[1].Value.Trim('`');
                // skip schema-qualified "db.table" -> use the table part
                var dot = name.LastIndexOf('.');
                if (dot >= 0) name = name[(dot + 1)..];
                if (!IsPlausibleTableName(name)) continue;
                counts[name] = counts.TryGetValue(name, out var c) ? c + 1 : 1;
            }
        }
        return counts;
    }

    static bool IsPlausibleTableName(string name)
    {
        // Skip aliases / variables / camelCase tokens; tables are lowercase_snake.
        if (name.Length == 0 || name.Any(c => !(char.IsLetterOrDigit(c) || c == '_'))) return false;
        if (char.IsUpper(name[0])) return false;          // alias like TaskProcess
        if (name.Any(char.IsUpper)) return false;
        if (SqlKeywords.Contains(name)) return false;     // e.g. "VALUES", "GROUP"
        return true;
    }

    /// <summary>Occurrences of Postgres-reserved words used as bare identifiers.</summary>
    public static List<(string File, string Word, string Sql)> ReservedIdentifierHits()
    {
        var hits = new List<(string, string, string)>();
        foreach (var occ in ExtractSql())
        {
            var sql = StripSqlStringLiterals(occ.Sql);
            var words = Regex.Matches(sql, @"[A-Za-z_][A-Za-z0-9_]*");
            foreach (Match w in words)
            {
                if (SqlKeywords.Contains(w.Value)) continue;
                if (PostgresReservedWords.Contains(w.Value))
                    hits.Add((occ.File, w.Value, Snippet(sql, w.Index)));
            }
        }
        return hits;
    }

    static string StripSqlStringLiterals(string sql)
    {
        // remove 'value' literals so reserved words inside data don't pollute
        return Regex.Replace(sql, "'(?:[^']|'')*'", "'?'");
    }

    static string Snippet(string s, int idx)
    {
        int start = Math.Max(0, idx - 30);
        int len = Math.Min(s.Length - start, 70);
        return s.Substring(start, len).Replace('\n', ' ').Trim();
    }

    public static Dictionary<string, int> MysqlOnlyCounts()
    {
        var counts = new Dictionary<string, int>();
        foreach (var occ in ExtractSql())
        {
            foreach (var kv in MysqlOnlyPatterns)
            {
                var n = Regex.Matches(occ.Sql, kv.Value, RegexOptions.IgnoreCase).Count;
                if (n > 0)
                    counts[kv.Key] = counts.TryGetValue(kv.Key, out var c) ? c + n : n;
            }
        }
        return counts;
    }

    public static Dictionary<string, int> BacktickUsagePerFile()
    {
        var map = new Dictionary<string, int>();
        foreach (var occ in ExtractSql())
        {
            var n = Regex.Matches(occ.Sql, MysqlOnlyPatterns["backtick"]).Count;
            if (n > 0)
                map[occ.File] = map.TryGetValue(occ.File, out var c) ? c + n : n;
        }
        return map;
    }

    public static Dictionary<string, int> MysqlTypeUsagePerFile()
    {
        var regex = new Regex(@"\bMySql(Connection|Command|Parameter|DataReader|Transaction|Exception|ConnectionStringBuilder)\b");
        var map = new Dictionary<string, int>();
        foreach (var file in AllCsFiles())
        {
            var text = File.ReadAllText(file);
            var n = regex.Matches(text).Count;
            if (n > 0)
                map[Rel(file)] = n;
        }
        return map;
    }
}
