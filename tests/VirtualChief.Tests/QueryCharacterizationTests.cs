using System;
using System.Threading.Tasks;
using MySqlConnector;
using VirtualChief.Tests.Support;
using Xunit;

namespace VirtualChief.Tests;

/// <summary>
/// Runs the ACTUAL SQL statements taken verbatim from the application sources
/// (App_Sources, Controllers, Eventi) against the seeded MariaDB to prove they
/// execute against the real schema. Read-only; any write path is wrapped in a
/// rolled-back transaction.
/// </summary>
public class QueryCharacterizationTests
{
    const string Kaizenkey = "kaizenkey";

    [Fact]
    public async Task DelaysAlarm_Query_Executes()
    {
        // Controllers/DelaysAlarmController.cs:29
        var sql = "SELECT taskID FROM tasksproduzione WHERE status <> 'F' ORDER BY lateStart";
        var n = await Db.RowCountAsync(Kaizenkey, sql);
        Assert.True(n >= 0, "delays query must execute");
    }

    [Fact]
    public async Task Ritardi_Query_Executes()
    {
        // Eventi/Ritardi.asmx.cs:38 (time portion replaced with a fixed instant)
        var when = "2021-06-15 12:00:00";
        var sql = "SELECT taskID FROM tasksproduzione WHERE status <> 'F' AND earlystart <= '" + when + "' ORDER BY lateStart";
        var n = await Db.RowCountAsync(Kaizenkey, sql);
        Assert.True(n >= 0);
    }

    [Fact]
    public async Task Warning_Query_Executes()
    {
        // Eventi/Warning.asmx.cs:32
        var sql = "SELECT taskID FROM registroeventiproduzione WHERE TipoEvento LIKE 'Warning' AND segnalato = false";
        var n = await Db.RowCountAsync(Kaizenkey, sql);
        Assert.True(n >= 0);
    }

    [Fact]
    public async Task ExpiryDate_Read_ReturnsRow()
    {
        // App_Sources/configurazione.cs:204
        var sql = "SELECT sezione, ID, parametro, valore FROM configurazione WHERE Sezione='Main' AND parametro = 'ExpiryDate'";
        var rows = await Db.QueryRowsAsync(Kaizenkey, sql, r => r.GetString(3));
        Assert.Single(rows);
        // value is stored as dd/MM/yyyy
        Assert.True(DateTime.TryParseExact(rows[0], "dd/MM/yyyy", null,
            System.Globalization.DateTimeStyles.None, out _), $"ExpiryDate not dd/MM/yyyy: '{rows[0]}'");
    }

    [Fact]
    public async Task TimeZone_Read_Executes()
    {
        // App_Sources/configurazione.cs:119
        var sql = "SELECT * FROM configurazione WHERE parametro LIKE 'TimeZone'";
        var n = await Db.RowCountAsync(Kaizenkey, sql);
        Assert.Equal(1, n);
    }

    [Fact]
    public async Task LoadTaskEvents_BigJoin_Executes()
    {
        // App_Sources/Analysis.cs:1700-1797 (loadTaskEvents) — the SIAV export query,
        // verbatim, including the bare `user` column reference.
        const string sql = @"SELECT anagraficaclienti.codice AS CustomerID,
anagraficaclienti.ragsociale AS CustomerName,anagraficaclienti.partitaiva AS CustomerVATNumber,
anagraficaclienti.codfiscale AS CustomerCodiceFiscale,
anagraficaclienti.indirizzo AS CustomerAddress,
anagraficaclienti.citta AS CustomerCity,
anagraficaclienti.provincia AS CustomerProvince,
anagraficaclienti.CAP AS CustomerZipCode,
anagraficaclienti.stato AS CustomerCountry,
anagraficaclienti.telefono AS CustomerPhoneNumber,
anagraficaclienti.email AS CustomerEMail,
anagraficaclienti.kanbanmanaged AS CustomerKanbanManaged,
commesse.idcommesse AS SalesOrderID,
commesse.anno AS SalesOrderYear,
commesse.cliente AS SalesOrderCustomer,
commesse.dataInserimento AS SalesOrderDate,
commesse.note AS SalesOrderNotes,
productionplan.id AS ProductionOrderID,
productionplan.anno AS ProductionOrderYear,
productionplan.processo AS ProductionOrderProductTypeID,
productionplan.revisione AS ProductionOrderProductTypeReview,
productionplan.variante AS ProductionOrderProductID,
productionplan.matricola AS ProductionOrderSerialNumber,
productionplan.status AS ProductionOrderStatus,
productionplan.reparto AS ProductionOrderDepartmentID,
productionplan.startTime AS ProductionOrderStartTime,
productionplan.dataConsegnaPrevista AS ProductionOrderDeliveryDate,
productionplan.dataPrevistaFineProduzione AS ProductionOrderEndProductionDate,
productionplan.planner AS ProductionOrderPlanner,
productionplan.quantita AS ProductionOrderQuantityOrdered,
productionplan.quantitaProdotta AS ProductionOrderQuantityProduced,
productionplan.kanbanCard AS ProductionOrderKanbanCardID,
processo.processID AS ProductTypeID,
processo.revisione AS ProductTypeReview,
processo.dataRevisione AS ProductTypeReviewDate,
processo.Name AS ProductTypeName,
processo.description AS ProductTypeDescription,
processo.attivo AS ProductTypeEnabled,
varianti.idvariante AS ProductID,
varianti.nomeVariante AS ProductName,
varianti.descVariante AS ProductDescription,
reparti.idreparto AS DepartmentID,
reparti.nome AS DepartmentName,
reparti.descrizione AS DepartmentDescription,
reparti.cadenza AS DepartmentTaktTime,
reparti.timezone AS DepartmentTimeZone,
productionplan.LeadTime AS ProductRealLeadTime,
productionplan.WorkingTime AS ProductRealWorkingTime,
productionplan.Delay AS ProductRealDelay,
productionplan.EndProductionDateReal AS ProductRealEndProductionDate,
tasksproduzione.TaskiD AS TaskID,
tasksproduzione.name AS TaskName,
tasksproduzione.description AS TaskDescription,
tasksproduzione.earlyStart As TaskEarlyStart,
tasksproduzione.lateStart AS TaskLateStart,
tasksproduzione.earlyFinish AS TaskEarlyFinish,
tasksproduzione.lateFinish AS TaskLateFinish,
tasksproduzione.status AS TaskStatus,
tasksproduzione.nOperatori AS TaskNumOperators,
tasksproduzione.qtaPrevista AS TaskQuantityOrdered,
tasksproduzione.qtaProdotta AS TaskQuantityProduced,
tempiciclo.setup AS TaskSetupTimePlanned,
tempiciclo.tempo AS TaskCycleTimePlanned,
tempiciclo.tunload AS TaskUnloadTimePlanned,
postazioni.idpostazioni AS WorkstationID,
postazioni.name AS WorkstationName,
postazioni.description AS WorkstationDescription,
tasksproduzione.endDateReal as TaskEndDateReal,
tasksproduzione.LeadTime AS TaskLeadTime,
tasksproduzione.WorkingTime AS TaskWorkingTime,
tasksproduzione.Delay AS TaskDelay,
tasksproduzione.OrigTask AS TaskOriginalTaskID,
tasksproduzione.RevOrigTask AS TaskOriginalTaskRev,
tasksproduzione.variante AS TaskOriginalTaskVar,
tasksproduzione.tempoCiclo AS TaskPlannedWorkingTime,
registroeventitaskproduzione.id,
registroeventitaskproduzione.data,
registroeventitaskproduzione.evento,
registroeventitaskproduzione.user
FROM anagraficaclienti INNER JOIN commesse ON(anagraficaclienti.codice = commesse.cliente)
INNER JOIN productionplan ON(commesse.anno = productionplan.annocommessa AND commesse.idcommesse = productionplan.commessa)
INNER JOIN reparti ON(reparti.idreparto = productionplan.reparto)
INNER JOIN varianti ON(varianti.idvariante = productionplan.variante)
INNER JOIN processo ON(processo.processid = productionplan.processo AND processo.revisione = productionplan.revisione)
INNER JOIN tasksproduzione ON(tasksproduzione.idarticolo = productionplan.id AND tasksproduzione.annoarticolo = productionplan.anno)
INNER JOIN processo AS TaskProcess ON(TaskProcess.processid = tasksproduzione.origtask AND TaskProcess.revisione = tasksproduzione.revorigtask)
INNER JOIN varianti AS TaskVariant ON(TaskVariant.idvariante = tasksproduzione.variante)
INNER JOIN postazioni ON(postazioni.idpostazioni = tasksproduzione.postazione)
INNER JOIN tempiciclo ON(tempiciclo.processo = tasksproduzione.origtask AND tempiciclo.revisione = tasksproduzione.revorigtask AND tasksproduzione.variante = tempiciclo.variante)
INNER JOIN registroeventitaskproduzione ON (registroeventitaskproduzione.task = tasksproduzione.taskid)
WHERE tasksproduzione.status = 'F' AND productionplan.status = 'F'
AND productionplan.EndProductionDateReal IS NOT NULL AND productionplan.EndProductionDateReal >= '2021-01-01 00:00:00'
AND productionplan.EndProductionDateReal <= '2021-12-31 23:59:59'
ORDER  BY productionplan.anno, productionplan.id asc, tasksproduzione.taskid, registroeventitaskproduzione.USER, registroeventitaskproduzione.data asc";

        var rows = await Db.QueryRowsAsync(Kaizenkey, sql, r => r.GetInt64(0));
        Assert.True(rows.Length >= 0);
    }

    [Fact]
    public async Task AliasCaseSensitivity_DifferenceIsDocumented()
    {
        // App_Sources/Analysis.cs:1790 uses "taskvariant" (lowercase alias) while the alias
        // is declared "TaskVariant". MySQL 8 (the app's real DB) resolves aliases
        // case-insensitively, so this works there. The test MariaDB is provisioned with
        // lower_case_table_names=1 to mirror MySQL 8's behaviour, so the ORIGINAL text now
        // also succeeds here. (PostgreSQL resolves aliases case-sensitively — still a real
        // difference to fix during the migration.)
        const string originalLower = "SELECT tasksproduzione.taskid FROM varianti AS TaskVariant JOIN tasksproduzione ON(taskvariant.idvariante = tasksproduzione.variante) LIMIT 1";
        const string corrected = "SELECT tasksproduzione.taskid FROM varianti AS TaskVariant JOIN tasksproduzione ON(TaskVariant.idvariante = tasksproduzione.variante) LIMIT 1";

        var ok = await Db.ScalarAsync(Kaizenkey, originalLower);
        Assert.NotNull(ok);

        var ok2 = await Db.ScalarAsync(Kaizenkey, corrected);
        Assert.NotNull(ok2);
    }

    [Fact]
    public async Task TaskHistoryView_IsQueryable()
    {
        var n = await Db.RowCountAsync("vc_dev", "SELECT * FROM taskproductionhistory");
        Assert.True(n >= 0, "taskproductionhistory view must be queryable");
    }

    [Fact]
    public async Task ExpiryDate_InsertPath_RollsBackCleanly()
    {
        // App_Sources/configurazione.cs:253-255 INSERT path; verified inside a
        // transaction that is rolled back so the seeded DB is untouched.
        const string testParam = "ExpiryDate_TEST";
        await using var conn = new MySqlConnection(Db.ConnectionString(Kaizenkey));
        await conn.OpenAsync();
        await using var tx = await conn.BeginTransactionAsync();
        await using var cmd = new MySqlCommand(
            "INSERT INTO configurazione(Sezione, ID, parametro, valore) VALUES('Main', -1, '" + testParam + "', '01/01/2030')", conn, tx);
        await cmd.ExecuteNonQueryAsync();
        await tx.RollbackAsync();

        var after = Convert.ToInt32(await Db.ScalarAsync(Kaizenkey,
            "SELECT COUNT(*) FROM configurazione WHERE parametro = '" + testParam + "'"));
        Assert.Equal(0, after);
    }
}
