using System;
using System.Data;
using System.Threading.Tasks;
using MySqlConnector;

namespace VirtualChief.Tests.Support;

/// <summary>
/// Connection settings for the characterization DB (a private MariaDB/MySQL instance).
/// Override via environment variables: VC_DB_HOST, VC_DB_PORT, VC_DB_USER, VC_DB_PASS.
/// </summary>
public static class Db
{
    public static string ConnectionString(string database)
    {
        var host = Env("VC_DB_HOST", "127.0.0.1");
        var port = Env("VC_DB_PORT", "3307");
        var user = Env("VC_DB_USER", "vc");
        var pass = Env("VC_DB_PASS", "vc");
        return $"Server={host};Port={port};Database={database};User ID={user};Password={pass};SslMode=None;AllowPublicKeyRetrieval=True;ConnectionTimeout=10;";
    }

    public static async Task<object?> ScalarAsync(string database, string sql)
    {
        await using var conn = new MySqlConnection(ConnectionString(database));
        await conn.OpenAsync();
        await using var cmd = new MySqlCommand(sql, conn);
        return await cmd.ExecuteScalarAsync();
    }

    public static async Task<int> RowCountAsync(string database, string sql)
    {
        await using var conn = new MySqlConnection(ConnectionString(database));
        await conn.OpenAsync();
        await using var cmd = new MySqlCommand($"SELECT COUNT(*) FROM ({sql}) AS _q", conn);
        var r = await cmd.ExecuteScalarAsync();
        return Convert.ToInt32(r);
    }

    public static async Task<T[]> QueryRowsAsync<T>(string database, string sql, Func<MySqlDataReader, T> map)
    {
        var results = new List<T>();
        await using var conn = new MySqlConnection(ConnectionString(database));
        await conn.OpenAsync();
        await using var cmd = new MySqlCommand(sql, conn);
        await using var rdr = await cmd.ExecuteReaderAsync();
        while (await rdr.ReadAsync())
            results.Add(map(rdr));
        return results.ToArray();
    }

    static string Env(string key, string def) => Environment.GetEnvironmentVariable(key) ?? def;
}
