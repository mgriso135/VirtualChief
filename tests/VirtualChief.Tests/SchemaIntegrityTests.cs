using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using VirtualChief.Tests.Support;
using Xunit;

namespace VirtualChief.Tests;

/// <summary>
/// Characterization tests against the loaded schema (private MariaDB on port 3307,
/// populated from kaizenkey.sql / vcmain.sql / vc_dev.sql).
/// Requires the DB to be provisioned (see provision-db.sh) and running.
/// </summary>
public class SchemaIntegrityTests
{
    static readonly string[] Databases = { "kaizenkey", "vcmain", "vc_dev" };

    [Fact]
    public async Task Databases_Exist()
    {
        var schemata = await Db.QueryRowsAsync("kaizenkey",
            "SELECT schema_name FROM information_schema.schemata", r => r.GetString(0));
        foreach (var db in Databases)
            Assert.Contains(db, schemata);
    }

    [Fact]
    public async Task Kaizenkey_HasExpectedTableCount()
    {
        var n = await Db.ScalarAsync("kaizenkey",
            "SELECT COUNT(*) FROM information_schema.tables WHERE table_schema='kaizenkey'");
        Assert.True(Convert.ToInt32(n) >= 95, $"expected >= 95 tables, found {n}");
    }

    [Fact]
    public async Task Kaizenkey_Views_ArePresent()
    {
        var n = await Db.ScalarAsync("kaizenkey",
            "SELECT COUNT(*) FROM information_schema.views WHERE table_schema='kaizenkey'");
        Assert.True(Convert.ToInt32(n) >= 9, $"expected >= 9 views, found {n}");
    }

    [Fact]
    public async Task Vcmain_Workspaces_AreSeeded()
    {
        var n = await Db.ScalarAsync("vcmain", "SELECT COUNT(*) FROM workspaces");
        Assert.True(Convert.ToInt32(n) >= 3, $"expected >= 3 workspaces, found {n}");
    }

    [Fact]
    public async Task Configurazione_HasExpectedMainRows()
    {
        var rows = await Db.QueryRowsAsync("kaizenkey",
            "SELECT parametro FROM configurazione WHERE Sezione='Main'",
            r => r.GetString(0));
        Assert.Contains("ExpiryDate", rows);
        Assert.Contains("Logo", rows);
        Assert.Contains("TimeZone", rows);
    }

    [Fact]
    public async Task AllCSharpTableReferences_ResolveToSchema()
    {
        var allTables = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var db in Databases)
        {
            var rows = await Db.QueryRowsAsync(db,
                "SELECT table_name FROM information_schema.tables WHERE table_schema='" + db + "'",
                r => r.GetString(0));
            foreach (var t in rows) allTables.Add(t);
        }

        var referenced = SqlAudit.TableReferences();
        var missing = referenced.Keys.Where(t => !allTables.Contains(t)).OrderBy(t => t).ToList();

        // Characterization: these 6 tables are referenced by C# SQL (inputpoints.cs,
        // parts.cs) but appear in NO schema dump (kaizenkey/vcmain/vc_dev). This is
        // schema drift to be resolved during migration (add missing DDL to the dump).
        var expectedMissing = new[]
        {
            "inputpoints", "inputpoints_departments", "inputpoints_workstations",
            "parts", "parts_suppliers", "taskspostazioni",
        };

        Assert.Equal(expectedMissing, missing);
    }

    [Fact]
    public void WebConfig_TenancyConnectionSwap_IsConsistent()
    {
        var doc = XDocument.Load(RepoPaths.WebConfig);
        var conns = doc.Descendants("add")
            .Where(a => (string?)a.Attribute("name") is "vcmain" or "masterDB")
            .ToDictionary(a => (string)a.Attribute("name")!, a => (string)a.Attribute("connectionString")!);

        Assert.Contains("vcmain", conns.Keys);
        Assert.Contains("masterDB", conns.Keys);

        // The tenancy mechanism: masterDB has an EMPTY database= that Dati.GetConnectionString
        // replaces with the tenant name (App_Sources/data.cs:43).
        var master = conns["masterDB"];
        Assert.True(master.Contains("database=;") || master.EndsWith("database="),
            "masterDB must carry an empty database= to be replaced per tenant");
        Assert.False(master.ToLower().Contains("database=vcmain"));

        foreach (var tenant in Databases)
        {
            var swapped = master.Replace("database=", "database=" + tenant);
            Assert.Contains("database=" + tenant, swapped, StringComparison.OrdinalIgnoreCase);
        }
    }
}
