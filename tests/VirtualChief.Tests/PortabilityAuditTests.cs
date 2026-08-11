using System.Linq;
using VirtualChief.Tests.Support;
using Xunit;

namespace VirtualChief.Tests;

/// <summary>
/// Static portability-audit tests.
///
/// These are CHARACTERIZATION / drift-detection tests: the baselines snapshot the
/// current inventory of MySQL-specific code. They pass today and fail only when the
/// code drifts, so they act as a checklist for the MySQL -> PostgreSQL migration.
/// Do not delete baseline entries; move the counter down as issues are fixed.
/// </summary>
public class PortabilityAuditTests
{
    [Fact]
    public void SqlStrings_AreExtracted()
    {
        var sql = SqlAudit.ExtractSql().ToList();
        Assert.True(sql.Count >= 1000, $"Expected >= 1000 SQL strings, found {sql.Count}");
        Assert.True(sql.Select(s => s.File).Distinct().Count() >= 25,
            "Expected SQL spread across >= 25 files");
    }

    [Fact]
    public void ReservedWordIdentifiers_MatchesBaseline()
    {
        var hits = SqlAudit.ReservedIdentifierHits().ToList();

        var words = hits.Select(h => h.Word.ToLowerInvariant()).Distinct().OrderBy(w => w).ToList();
        Assert.Equal(new[] { "user" }, words);

        var files = hits.Select(h => h.File).Distinct().OrderBy(f => f).ToList();
        Assert.Equal(new[]
        {
            "KisWebApp/App_Sources/Analysis.cs",
            "KisWebApp/App_Sources/clienti.cs",
            "KisWebApp/App_Sources/commesse.cs",
            "KisWebApp/App_Sources/configurazione.cs",
            "KisWebApp/App_Sources/data.cs",
            "KisWebApp/App_Sources/processi.cs",
            "KisWebApp/App_Sources/produzione.cs",
            "KisWebApp/App_Sources/quality.cs",
            "KisWebApp/App_Sources/users.cs",
            "KisWebApp/App_Sources/WorkInstructions.cs",
        }, files);

        // 76 bare `user` identifiers are the Postgres migration surface.
        Assert.Equal(76, hits.Count);
    }

    [Fact]
    public void MysqlOnlyConstructs_MatchesBaseline()
    {
        var counts = SqlAudit.MysqlOnlyCounts();
        Assert.Equal(3, counts.Count);
        Assert.Equal(1, counts["backtick"]);
        Assert.Equal(5, counts["LAST_INSERT_ID"]);
        Assert.Equal(1, counts["NOW()"]);
    }

    [Fact]
    public void BacktickUsage_OnlyInConfigurazione()
    {
        var bt = SqlAudit.BacktickUsagePerFile();
        Assert.Equal(new[] { "KisWebApp/App_Sources/configurazione.cs" }, bt.Keys.OrderBy(k => k).ToArray());
        Assert.Equal(1, bt["KisWebApp/App_Sources/configurazione.cs"]);
    }

    [Fact]
    public void MysqlConcreteTypes_MatchesBaseline()
    {
        var mt = SqlAudit.MysqlTypeUsagePerFile();

        Assert.Equal(28, mt.Count);
        Assert.Equal(3080, mt.Values.Sum());

        Assert.Equal(199, mt["KisWebApp/App_Sources/Account.cs"]);
        Assert.Equal(24, mt["KisWebApp/App_Sources/Analysis.cs"]);
        Assert.Equal(103, mt["KisWebApp/App_Sources/andon.cs"]);
        Assert.Equal(123, mt["KisWebApp/App_Sources/clienti.cs"]);
        Assert.Equal(170, mt["KisWebApp/App_Sources/commesse.cs"]);
        Assert.Equal(434, mt["KisWebApp/App_Sources/configurazione.cs"]);
        Assert.Equal(9, mt["KisWebApp/App_Sources/data.cs"]);
        Assert.Equal(138, mt["KisWebApp/App_Sources/eventi.cs"]);
        Assert.Equal(119, mt["KisWebApp/App_Sources/FreeTimeMeasurement.cs"]);
        Assert.Equal(63, mt["KisWebApp/App_Sources/inputpoints.cs"]);
        Assert.Equal(15, mt["KisWebApp/App_Sources/KanbanBox.cs"]);
        Assert.Equal(53, mt["KisWebApp/App_Sources/kpi.cs"]);
        Assert.Equal(33, mt["KisWebApp/App_Sources/menu.cs"]);
        Assert.Equal(28, mt["KisWebApp/App_Sources/NoProductiveTasks.cs"]);
        Assert.Equal(31, mt["KisWebApp/App_Sources/parts.cs"]);
        Assert.Equal(44, mt["KisWebApp/App_Sources/permessi.cs"]);
        Assert.Equal(71, mt["KisWebApp/App_Sources/postazioni.cs"]);
        Assert.Equal(363, mt["KisWebApp/App_Sources/processi.cs"]);
        Assert.Equal(228, mt["KisWebApp/App_Sources/produzione.cs"]);
        Assert.Equal(293, mt["KisWebApp/App_Sources/quality.cs"]);
        Assert.Equal(10, mt["KisWebApp/App_Sources/relazioni.cs"]);
        Assert.Equal(182, mt["KisWebApp/App_Sources/reparti.cs"]);
        Assert.Equal(246, mt["KisWebApp/App_Sources/users.cs"]);
        Assert.Equal(82, mt["KisWebApp/App_Sources/WorkInstructions.cs"]);
        Assert.Equal(6, mt["KisWebApp/Controllers/DelaysAlarmController.cs"]);
        Assert.Equal(6, mt["KisWebApp/Eventi/Ritardi.asmx.cs"]);
        Assert.Equal(3, mt["KisWebApp/Eventi/Warning.asmx.cs"]);
        Assert.Equal(4, mt["VCProductionEventsExport-SIAV/Program.cs"]);
    }

    [Fact]
    public void ReferencedTables_DistinctCount_MatchesBaseline()
    {
        var tables = SqlAudit.TableReferences();
        Assert.Equal(98, tables.Count);
        // Sanity: core tables are referenced
        foreach (var core in new[] { "anagraficaclienti", "commesse", "productionplan",
                     "tasksproduzione", "configurazione", "processo", "varianti", "reparti" })
            Assert.True(tables.ContainsKey(core), $"expected {core} to be referenced");
    }
}
