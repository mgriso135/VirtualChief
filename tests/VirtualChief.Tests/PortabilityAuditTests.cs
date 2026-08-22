using System;
using System.IO;
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

        // 80 bare `user` identifiers are the Postgres migration surface.
        Assert.Equal(80, hits.Count);
    }

    [Fact]
    public void MysqlOnlyConstructs_MatchesBaseline()
    {
        var counts = SqlAudit.MysqlOnlyCounts();
        Assert.Equal(3, counts.Count);
        Assert.Equal(4, counts["backtick"]);
        Assert.Equal(5, counts["LAST_INSERT_ID"]);
        Assert.Equal(1, counts["NOW()"]);
    }

    [Fact]
    public void BacktickUsage_OnlyInConfigurazione()
    {
        var bt = SqlAudit.BacktickUsagePerFile();
        Assert.Equal(new[] { "KisWebApp/App_Sources/configurazione.cs" }, bt.Keys.OrderBy(k => k).ToArray());
        Assert.Equal(4, bt["KisWebApp/App_Sources/configurazione.cs"]);
    }

[Fact]
    public void MysqlConcreteTypes_MatchesBaseline()
    {
        var mt = SqlAudit.MysqlTypeUsagePerFile();

        Assert.Equal(27, mt.Count);
        Assert.Equal(1189, mt.Values.Sum());

        Assert.Equal(94, mt["KisWebApp/App_Sources/Account.cs"]);
        Assert.Equal(8, mt["KisWebApp/App_Sources/Analysis.cs"]);
        Assert.Equal(68, mt["KisWebApp/App_Sources/clienti.cs"]);
        Assert.Equal(7, mt["KisWebApp/App_Sources/data.cs"]);
        Assert.Equal(72, mt["KisWebApp/App_Sources/eventi.cs"]);
        Assert.Equal(84, mt["KisWebApp/App_Sources/commesse.cs"]);
        Assert.Equal(208, mt["KisWebApp/App_Sources/configurazione.cs"]);
        Assert.Equal(58, mt["KisWebApp/App_Sources/FreeTimeMeasurement.cs"]);
        Assert.Equal(5, mt["KisWebApp/App_Sources/KanbanBox.cs"]);
        Assert.Equal(12, mt["KisWebApp/App_Sources/kpi.cs"]);
        Assert.Equal(15, mt["KisWebApp/App_Sources/menu.cs"]);
        Assert.Equal(14, mt["KisWebApp/App_Sources/NoProductiveTasks.cs"]);
        Assert.Equal(14, mt["KisWebApp/App_Sources/parts.cs"]);
        Assert.Equal(11, mt["KisWebApp/App_Sources/permessi.cs"]);
        Assert.Equal(159, mt["KisWebApp/App_Sources/processi.cs"]);
        Assert.Equal(1, mt["KisWebApp/App_Sources/produzione.cs"]);
        Assert.Equal(152, mt["KisWebApp/App_Sources/quality.cs"]);
        Assert.Equal(2, mt["KisWebApp/App_Sources/relazioni.cs"]);
        Assert.Equal(86, mt["KisWebApp/App_Sources/reparti.cs"]);
        Assert.Equal(106, mt["KisWebApp/App_Sources/users.cs"]);
        Assert.Equal(2, mt["KisWebApp/Controllers/DelaysAlarmController.cs"]);
        Assert.Equal(1, mt["KisWebApp/Controllers/WarningController.cs"]);
        Assert.Equal(2, mt["KisWebApp/Controllers/RitardiController.cs"]);
        Assert.Equal(2, mt["KisWebApp/Eventi/Ritardi.asmx.cs"]);
        Assert.Equal(1, mt["KisWebApp/Eventi/Warning.asmx.cs"]);
        Assert.Equal(3, mt["VCProductionEventsExport-SIAV/Program.cs"]);
        // TenantDatabaseGate: workspace existence probe (MySqlConnection + MySqlConnectionStringBuilder)
        Assert.Equal(2, mt["VirtualChief/Pages/TenantDatabaseGate.cs"]);
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

    [Fact]
    public void AspChartControls_AreEliminated()
    {
        // Phase 2 milestone: all <asp:Chart> server-rendered chart controls have been
        // replaced with client-side Google Charts / D3 renderings.
        var usage = SqlAudit.WebFormsControlUsage();
        var chartFiles = usage
            .Where(kv => kv.Value.ContainsKey("asp:Chart"))
            .Select(kv => kv.Key)
            .OrderBy(f => f, StringComparer.OrdinalIgnoreCase)
            .ToArray();
        Assert.Empty(chartFiles);
    }

    [Fact]
    public void WebFormsPages_HaveRazorCounterpart()
    {
        // Phase 2 milestone: every WebForms page/control (..aspx/..ascx) must have a
        // same-basename Razor (.cshtml) counterpart before the WebForms files are retired.
        var missing = SqlAudit.WebFormsWithoutRazorCounterpart();
        Assert.Empty(missing);
    }

    /// <summary>
    /// Every legacy ASMX SOAP endpoint must have a ported Web API controller so that
    /// scheduler/console agents keep a callable contract while the .asmx files are
    /// retired during Phase 2.
    /// </summary>
    [Fact]
    public void AsmxServices_HavePortingController()
    {
        var asmxToController = new[]
        {
            ("KisWebApp/Eventi/Licensing.asmx", "KisWebApp/Controllers/LicensingController.cs"),
            ("KisWebApp/Eventi/Warning.asmx", "KisWebApp/Controllers/WarningController.cs"),
            ("KisWebApp/Eventi/Ritardi.asmx", "KisWebApp/Controllers/RitardiController.cs"),
            ("KisWebApp/Eventi/QualityModuleEvents.asmx", "KisWebApp/Controllers/QualityModuleEventsController.cs"),
            ("KisWebApp/KanbanBox/KanbanBoxReader.asmx", "KisWebApp/Controllers/KanbanBoxReaderController.cs"),
            ("KisWebApp/KanbanBox/KanbanBoxCheckHealth.asmx", "KisWebApp/Controllers/KanbanBoxCheckHealthController.cs"),
            ("KisWebApp/Processi/getProcessData.asmx", "KisWebApp/Controllers/ProcessDataController.cs"),
        };

        var root = RepoPaths.Root;
        foreach (var (asmx, controller) in asmxToController)
        {
            var asmxPath = Path.Combine(root, asmx);
            var ctrlPath = Path.Combine(root, controller);

            Assert.True(File.Exists(asmxPath), $"ASMX source missing: {asmx}");
            Assert.True(File.Exists(ctrlPath), $"No Web API controller for {asmx}; expected {controller}");

            // The controller must be a real ApiController (not an empty stub) that
            // references the legacy service it replaces.
            var src = File.ReadAllText(ctrlPath);
            Assert.Contains("ApiController", src, StringComparison.Ordinal);
            Assert.Contains(Path.GetFileName(asmx), src, StringComparison.Ordinal);
        }
    }
}
