using System;
using System.IO;
using System.Threading.Tasks;
using MySqlConnector;
using VirtualChief.Tests.Support;
using Xunit;

namespace VirtualChief.Tests;

/// <summary>
/// Workspace database gate. Bug being fixed: a workspace registered in
/// vcmain.workspaces WITHOUT its own schema made every page render
/// half-broken (empty lists) while each page swallowed MySqlException
/// "Unknown database". The gate (VirtualChief/Pages/TenantDatabaseGate.cs)
/// must detect the missing schema BEFORE any page runs and block everything.
///
/// These tests characterize the existence probe (connect WITHOUT selecting a
/// default schema, count matching rows in information_schema.SCHEMATA — SQL
/// kept verbatim in sync with TenantDatabaseChecker.ProbeAsync) and guard the
/// pipeline wiring.
/// </summary>
public class TenantDatabaseGateTests
{
    const string ProbeSql =
        "SELECT COUNT(*) FROM information_schema.SCHEMATA WHERE SCHEMA_NAME = @db";

    private static async Task<int> CountSchemasAsync(string schemaName)
    {
        // Same connection shape as the app probe: NO default database selected.
        var csb = new MySqlConnectionStringBuilder(Db.ConnectionString("unused"))
        {
            Database = "",
        };
        await using var conn = new MySqlConnection(csb.ConnectionString);
        await conn.OpenAsync();
        await using var cmd = new MySqlCommand(ProbeSql, conn);
        cmd.Parameters.AddWithValue("@db", schemaName);
        var r = await cmd.ExecuteScalarAsync();
        return Convert.ToInt32(r);
    }

    [Fact]
    public async Task Probe_SeededTenant_Found()
    {
        Assert.True(await CountSchemasAsync("kaizenkey") > 0,
            "seeded tenant 'kaizenkey' must be found by the probe");
    }

    [Fact]
    public async Task Probe_MissingDatabase_NotFound()
    {
        Assert.Equal(0, await CountSchemasAsync("__definitely_missing_db__"));
    }

    [Fact]
    public void Pipeline_RegistersGate_AfterAuthorization_AndErrorPageExists()
    {
        var program = File.ReadAllText(
            Path.Combine(RepoPaths.Root, "VirtualChief", "Program.cs"));

        var authIdx = program.IndexOf("app.UseAuthentication();", StringComparison.Ordinal);
        var authzIdx = program.IndexOf("app.UseAuthorization();", StringComparison.Ordinal);
        var gateIdx = program.IndexOf("UseMiddleware<TenantDatabaseGateMiddleware>", StringComparison.Ordinal);
        Assert.True(authIdx >= 0 && authzIdx >= 0, "authentication/authorization wiring missing");
        Assert.True(gateIdx > authzIdx,
            "the gate middleware must run after authentication/authorization");

        Assert.True(File.Exists(
                Path.Combine(RepoPaths.Root, "VirtualChief", "Pages", "Error.cshtml")),
            "/Error must exist: UseExceptionHandler and the gate redirect target it");
    }
}
