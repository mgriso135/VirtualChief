using System;
using Xunit;
using Dati;
using KIS.App_Code;
using KIS.App_Sources;

namespace VirtualChief.DomainTests;

/// <summary>
/// Tier-1 business-flow tests: exercise the real (unmodified) legacy App_Sources
/// classes compiled on Linux via the shims, against the private MariaDB instance
/// provisioned by tests/provision-db.sh.
/// </summary>
public class DomainBusinessTests
{
    const string Tenant = "vc_dev";

    [Fact]
    public void Dati_GetConnectionString_SwapsTenantIntoDatabase()
    {
        var cs = new Dati.Dati().GetConnectionString(Tenant);
        Assert.Contains("database=" + Tenant, cs);
    }

    [Fact]
    public void KISConfig_ExpiryDate_LoadsFromConfigurazione()
    {
        var cfg = new KISConfig(Tenant);
        Assert.Equal(new DateTime(2020, 11, 1), cfg.ExpiryDate.Date);
    }

    [Fact]
    public void KISConfig_WizLogoCompleted_TrueWhenLogoConfigured()
    {
        var cfg = new KISConfig(Tenant);
        Assert.True(cfg.WizLogoCompleted);
    }

    [Fact]
    public void KISConfig_SalesOrderImportFrom3PartySystem_RoundTrips()
    {
        var cfg = new KISConfig(Tenant);
        var original = cfg.SalesOrderImportFrom3PartySystem;
        try
        {
            cfg.SalesOrderImportFrom3PartySystem = true;
            Assert.True(cfg.SalesOrderImportFrom3PartySystem);
            cfg.SalesOrderImportFrom3PartySystem = false;
            Assert.False(cfg.SalesOrderImportFrom3PartySystem);
        }
        finally
        {
            cfg.SalesOrderImportFrom3PartySystem = original;
        }
    }

    [Fact]
    public void ConfigBaseOrderStatusReport_LoadsDefaultFlagsWhenNoRows()
    {
        var cfg = new configBaseOrderStatusReport(Tenant);
        Assert.True(cfg.IDCommessa);
        Assert.True(cfg.Ritardo);
        Assert.True(cfg.Task_TempoCiclo);
        Assert.True(cfg.Task_QuantitaProdotta);
    }

    [Fact]
    public void MeasurementUnits_LoadsUnitsFromSeed()
    {
        var units = new MeasurementUnits(Tenant);
        units.loadMeasurementUnits();
        Assert.NotEmpty(units.UnitsList);
        Assert.True(units.UnitsList[0].ID > 0);
        Assert.True(units.UnitsList[0].Type.Length > 0);
    }

    [Fact]
    public void HomeBoxesList_LoadsFromSeed()
    {
        var el = new HomeBoxesList(Tenant);
        Assert.NotEmpty(el.Elenco);
        Assert.Equal("Prodotti programmati", el.Elenco[0].Nome);
    }

    [Fact]
    public void WizardConfig_InterfacciaPERT_DefaultsToGraphWhenUnset()
    {
        var cfg = new WizardConfig(Tenant);
        Assert.Equal("Graph", cfg.interfacciaPERT);
    }

    [Fact]
    public void ElencoCommesse_LoadsWorkOrders()
    {
        var elenco = new ElencoCommesse(Tenant);
        elenco.loadCommesse();
        Assert.NotEmpty(elenco.Commesse);
    }

    [Fact]
    public void Reparto_ById_LoadsName()
    {
        var rp = new Reparto(Tenant, 1);
        Assert.Equal(1, rp.id);
        Assert.Equal("Alluminio", rp.name);
    }

    [Fact]
    public void Reparto_LoadPostazioni_ReturnsStations()
    {
        var rp = new Reparto(Tenant, 1);
        rp.loadPostazioni();
        Assert.NotNull(rp.Postazioni);
    }

    [Fact]
    public void Reparto_LoadProcessiVarianti_Runs()
    {
        var rp = new Reparto(Tenant, 1);
        rp.loadProcessiVarianti();
        Assert.NotNull(rp.processiVarianti);
    }

    [Fact]
    public void Reparto_LoadOperatori_Runs()
    {
        var rp = new Reparto(Tenant, 1);
        rp.loadOperatori();
        Assert.NotNull(rp.Operatori);
    }

    [Fact]
    public void Relations_LoadAllFromSeed_OrderedByName()
    {
        var rels = new relations(Tenant);
        Assert.Equal(4, rels.numRelations);
        Assert.NotNull(rels.list);
        Assert.Equal(4, rels.list.Length);
        Assert.Equal("#ND", rels.list[0].Name);
        Assert.Equal("FIFO lane", rels.list[1].Name);
        Assert.Equal("Pull", rels.list[2].Name);
        Assert.Equal("Push", rels.list[3].Name);
    }

    [Fact]
    public void Relazione_ById_LoadsFromSeed()
    {
        var rel = new relazione(Tenant, 1);
        Assert.Equal(1, rel.relationID);
        Assert.Equal("Pull", rel.Name);
        Assert.Equal("/img/pull.gif", rel.imgURL);
    }

    [Fact]
    public void Relazione_UnknownId_ReturnsEmptyInstance()
    {
        var rel = new relazione(Tenant, 9999);
        Assert.Equal(-1, rel.relationID);
        Assert.Equal("", rel.Name);
    }

    [Fact]
    public void Permission_ById_LoadsFromVCMain()
    {
        var p = new Permission(4);
        Assert.Equal(4, p.ID);
        Assert.Equal("Postazione check-in", p.Nome);
    }

    [Fact]
    public void PermissionsList_LoadsAll()
    {
        var list = new PermissionsList();
        Assert.NotEmpty(list.Elenco);
        Assert.True(list.Elenco.Exists(p => p.ID == 5 && p.Nome == "Task Produzione"));
    }

    [Fact]
    public void VoceMenu_ById_LoadsFromVCMain()
    {
        var vm = new VoceMenu(2);
        Assert.Equal(2, vm.ID);
        Assert.Equal("Admin", vm.Titolo);
    }

    [Fact]
    public void VoceMenu_UnknownId_ReturnsMinusOne()
    {
        var vm = new VoceMenu(999999);
        Assert.Equal(-1, vm.ID);
    }

    [Fact]
    public void VoceMenu_LoadFigli_ReturnsChildren()
    {
        var vm = new VoceMenu(2);
        vm.loadFigli();
        Assert.NotEmpty(vm.VociFiglie);
    }

    [Fact]
    public void MainMenu_LoadsRootItems()
    {
        var m = new MainMenu(Tenant);
        Assert.NotEmpty(m.Elenco);
    }

    [Fact]
    public void AppConfig_Smtp_FallsBackToConfig()
    {
        var prev = Environment.GetEnvironmentVariable("VC_SMTP_USER");
        Environment.SetEnvironmentVariable("VC_SMTP_USER", null);
        try
        {
            Assert.Equal("test-smtp@example.test", AppConfig.SmtpUsername);
        }
        finally
        {
            Environment.SetEnvironmentVariable("VC_SMTP_USER", prev);
        }
    }

    [Fact]
    public void AppConfig_Smtp_EnvOverridesConfig()
    {
        var prev = Environment.GetEnvironmentVariable("VC_SMTP_USER");
        Environment.SetEnvironmentVariable("VC_SMTP_USER", "env-smtp@example.test");
        try
        {
            Assert.Equal("env-smtp@example.test", AppConfig.SmtpUsername);
        }
        finally
        {
            Environment.SetEnvironmentVariable("VC_SMTP_USER", prev);
        }
    }

    [Fact]
    public void AppConfig_Auth0_ReadsFromConfig()
    {
        Assert.Equal("test.eu.auth0.com", AppConfig.Auth0Domain);
    }

    [Fact]
    public void WebEnv_OutsideWebRequest_ReturnsDefaults()
    {
        var prev = System.Web.HttpContext.Current;
        System.Web.HttpContext.Current = null;
        try
        {
            Assert.Equal("", WebEnv.ActiveWorkspaceName);
            Assert.Equal(-1, WebEnv.ActiveWorkspaceId);
            var d = new Dati.Dati();
            Assert.Equal("", d.getActiveWorkspaceName());
            Assert.Equal(-1, d.getActiveWorkspaceId());
        }
        finally
        {
            System.Web.HttpContext.Current = prev;
        }
    }

    [Fact]
    public void WebEnv_EmptyContext_ReturnsDefaults()
    {
        var prev = System.Web.HttpContext.Current;
        System.Web.HttpContext.Current = new System.Web.HttpContext();
        try
        {
            Assert.Equal("", WebEnv.ActiveWorkspaceName);
            Assert.Equal(-1, WebEnv.ActiveWorkspaceId);
        }
        finally
        {
            System.Web.HttpContext.Current = prev;
        }
    }

    [Fact]
    public void WebEnv_GeneratePassword_ReturnsRequestedLength()
    {
        string pwd = WebEnv.GeneratePassword(16, 2);
        Assert.Equal(16, pwd.Length);
        Assert.NotNull(pwd);
    }

    [Fact]
    public void Processo_ById_LoadsFromSeed()
    {
        var prc = new processo(Tenant, 5);
        Assert.Equal(5, prc.processID);
        Assert.Equal("CNC THORWESTEN - GRAF", prc.processName);
        Assert.Equal(0, prc.revisione);
        Assert.True(prc.attivo);
    }

    [Fact]
    public void Processo_ByName_LoadsFromSeed()
    {
        var prc = new processo(Tenant, "CNC THORWESTEN - GRAF");
        Assert.Equal(5, prc.processID);
    }

    [Fact]
    public void Processo_ByIdAndRev_LoadsFromSeed()
    {
        var prc = new processo(Tenant, 5, 0);
        Assert.Equal(5, prc.processID);
        Assert.Equal("CNC THORWESTEN - GRAF", prc.processName);
    }

    [Fact]
    public void Processo_UnknownId_ReturnsMinusOne()
    {
        var prc = new processo(Tenant, 99999);
        Assert.Equal(-1, prc.processID);
        Assert.Equal("", prc.processName);
    }

    [Fact]
    public void Processo_LoadPadre_LoadsFromSeed()
    {
        var prc = new processo(Tenant, 5);
        prc.loadPadre();
        Assert.Equal(0, prc.processoPadre);
        Assert.Equal(0, prc.revPadre);
    }

    [Fact]
    public void Processo_LoadVarianti_LoadsFromSeed()
    {
        var prc = new processo(Tenant, 0);
        prc.loadVarianti();
        Assert.Equal(91, prc.variantiProcesso.Count);
    }

    [Fact]
    public void Processo_LoadFigli_ReturnsChildren()
    {
        var prc = new processo(Tenant, 16);
        prc.loadFigli();
        Assert.Equal(6, prc.subProcessi.Count);
    }

    [Fact]
    public void Processo_LoadPrecedenti_LoadsFromSeed()
    {
        var prc = new processo(Tenant, 5);
        prc.loadPrecedenti();
        Assert.Equal(91, prc.processiPrec.Count);
        Assert.Equal(91, prc.PreviousTasks.Count);
    }

    [Fact]
    public void Processo_LoadSuccessivi_LoadsFromSeed()
    {
        var prc = new processo(Tenant, 5);
        prc.loadSuccessivi();
        Assert.Equal(119, prc.processiSucc.Count);
        Assert.Equal(119, prc.FollowingTasks.Count);
    }

    [Fact]
    public void Processo_LoadKPIs_ReturnsZeroWhenNoKPIs()
    {
        var prc = new processo(Tenant, 5);
        prc.loadKPIs();
        Assert.Equal(0, prc.numKPIs);
    }

    [Fact]
    public void Processo_LoadPostazioniTask_Runs()
    {
        var prc = new processo(Tenant, 5);
        prc.loadPostazioniTask();
        Assert.NotNull(prc.elencoPostazioniTask);
    }

    [Fact]
    public void ElencoProcessi_LoadsFromSeed()
    {
        var el = new ElencoProcessi(Tenant);
        Assert.NotEmpty(el.Elenco);
        Assert.Contains(el.Elenco, p => p.processID == 5);
    }

    [Fact]
    public void Variante_ById_LoadsFromSeed()
    {
        var vr = new variante(Tenant, 1);
        Assert.Equal(1, vr.idVariante);
        Assert.Equal("Serramenti PVC", vr.nomeVariante);
    }

    [Fact]
    public void ElencoVarianti_LoadsFromSeed()
    {
        var el = new elencoVarianti(Tenant);
        Assert.Equal(158, el.numVarianti);
        Assert.Equal(158, el.elenco.Length);
    }

    [Fact]
    public void ProcessoVariante_ByIds_LoadsFromSeed()
    {
        var prc = new processo(Tenant, 0);
        var vr = new variante(Tenant, 1);
        var pv = new ProcessoVariante(Tenant, prc, vr);
        Assert.NotNull(pv.process);
        Assert.Equal(0, pv.process.processID);
    }

    [Fact]
    public void ProcessoVariante_UnknownPair_ReturnsNullProcess()
    {
        var prc = new processo(Tenant, 5);
        var vr = new variante(Tenant, 1);
        var pv = new ProcessoVariante(Tenant, prc, vr);
        Assert.Null(pv.process);
    }

    [Fact]
    public void TempiCiclo_LoadsTempiFromSeed()
    {
        var tc = new TempiCiclo(Tenant, 0, 0, 1);
        Assert.NotNull(tc.Tempi);
    }

    [Fact]
    public void ElencoTasks_LoadsProcessesFromSeed()
    {
        var el = new ElencoTasks(Tenant);
        Assert.NotEmpty(el.Elenco);
    }

    [Fact]
    public void User_ByUsername_LoadsAdminFromSeed()
    {
        var usr = new User(Tenant, "admin");
        Assert.Equal("admin", usr.username);
        Assert.Equal(0, usr.ID);
        Assert.Equal("Admin", usr.name);
        Assert.Equal("KaizenGenius", usr.cognome);
        Assert.Equal("Admin", usr.typeOfUser);
        Assert.True(usr.Enabled);
    }

    [Fact]
    public void User_ByID_LoadsAdminFromSeed()
    {
        var usr = new User(Tenant, 0);
        Assert.Equal("admin", usr.username);
        Assert.Equal(0, usr.ID);
    }

    [Fact]
    public void User_UnknownUsername_ReturnsEmpty()
    {
        var usr = new User(Tenant, "nonexistentuser");
        Assert.Equal("", usr.username);
        Assert.False(usr.Enabled);
    }

    [Fact]
    public void User_UnknownID_ReturnsEmpty()
    {
        var usr = new User(Tenant, 99999);
        Assert.Equal("", usr.username);
        Assert.False(usr.Enabled);
    }

    [Fact]
    public void User_LoadGruppi_LoadsAdminGroups()
    {
        var usr = new User(Tenant, "admin");
        Assert.True(usr.loadGruppi());
        Assert.NotNull(usr.Gruppi);
        Assert.Contains(usr.Gruppi, g => g.ID == 0);
        Assert.Contains(usr.Gruppi, g => g.ID == 5);
    }

    [Fact]
    public void User_LoadPostazioniAttive_LoadsSeed()
    {
        var usr = new User(Tenant, "admin");
        usr.loadPostazioniAttive();
        Assert.NotNull(usr.PostazioniAttive);
        Assert.Equal(3, usr.PostazioniAttive.Count);
    }

    [Fact]
    public void User_LoadTaskAvviati_ReturnsEmptyWhenNoStartedTasks()
    {
        var usr = new User(Tenant, "admin");
        usr.loadTaskAvviati();
        Assert.NotNull(usr.TaskAvviati);
        Assert.Empty(usr.TaskAvviati);
    }

    [Fact]
    public void User_UserExists_TrueForExisting()
    {
        var usr = new User(Tenant);
        Assert.True(usr.UserExists("admin"));
    }

    [Fact]
    public void User_UserExists_FalseForUnknown()
    {
        var usr = new User(Tenant);
        Assert.False(usr.UserExists("nonexistentuser"));
    }

    [Fact]
    public void User_LoadCustomer_EmptyForAdmin()
    {
        var usr = new User(Tenant, "admin");
        usr.loadCustomer();
        Assert.NotNull(usr.Customers);
        Assert.Empty(usr.Customers);
    }

    [Fact]
    public void User_SegnalazioneRitardiReparto_LoadsAdminReparti()
    {
        var usr = new User(Tenant, "admin");
        var ret = usr.SegnalazioneRitardiReparto;
        Assert.NotEmpty(ret);
        Assert.Contains(ret, r => r.id == 1);
    }

    [Fact]
    public void User_SegnalazioneWarningReparto_LoadsAdminReparti()
    {
        var usr = new User(Tenant, "admin");
        var ret = usr.SegnalazioneWarningReparto;
        Assert.NotEmpty(ret);
        Assert.Contains(ret, r => r.id == 1);
    }

    [Fact]
    public void User_SegnalazioneRitardiCommessa_ReturnsEmpty()
    {
        var usr = new User(Tenant, "admin");
        Assert.Empty(usr.SegnalazioneRitardiCommessa);
    }

    [Fact]
    public void User_SegnalazioneRitardiArticolo_ReturnsEmpty()
    {
        var usr = new User(Tenant, "admin");
        Assert.Empty(usr.SegnalazioneRitardiArticolo);
    }

    [Fact]
    public void User_Activate_WrongChecksum_ReturnsFalse()
    {
        var usr = new User(Tenant, "admin");
        Assert.False(usr.Activate("admin", "wrongchecksum"));
    }

    [Fact]
    public void User_LoadExecutableTasks_Runs()
    {
        var usr = new User(Tenant, "admin");
        usr.LoadExecutableTasks();
        Assert.NotNull(usr.ExecutableTasks);
    }

    [Fact]
    public void UserList_LoadsFromSeed_EmptyDueToLegacyQuirk()
    {
        var ul = new UserList("21");
        Assert.NotNull(ul.listUsers);
        Assert.Empty(ul.listUsers);
    }

    [Fact]
    public void NonCompliance_LoadsExistingRowFromSeed()
    {
        var nc = new NonCompliance(Tenant, 0, 2019);
        Assert.Equal(0, nc.ID);
        Assert.Equal(2019, nc.Year);
        Assert.Equal(1, nc.Quantity);
        Assert.Equal('O', nc.Status);
        Assert.Equal(0.0, nc.Cost);
        Assert.Equal("admin", nc.UserID);
        Assert.Equal("Test NC", nc.Description);
    }

    [Fact]
    public void NonCompliance_MissingRow_LeavesIdAtMinusOne()
    {
        var nc = new NonCompliance(Tenant, 99999, 2019);
        Assert.Equal(-1, nc.ID);
        Assert.Equal(-1, nc.Year);
    }

    [Fact]
    public void NonCompliance_CategoryLoad_ReturnsEmptyWhenNoneLinked()
    {
        var nc = new NonCompliance(Tenant, 1, 2019);
        nc.CategoryLoad();
        Assert.NotNull(nc.Categories);
        Assert.Empty(nc.Categories);
    }

    [Fact]
    public void NonCompliance_ProductsLoad_ReturnsSeedProducts()
    {
        var nc = new NonCompliance(Tenant, 0, 2019);
        nc.ProductsLoad();
        Assert.Equal(2, nc.Products.Count);
        Assert.Contains(nc.Products, p => p.ProductID == 302 && p.ProductYear == 2019);
        Assert.Contains(nc.Products, p => p.ProductID == 1937 && p.ProductYear == 2019);
    }

    [Fact]
    public void NonCompliances_LoadsTwoSeedRows()
    {
        var list = new NonCompliances(Tenant);
        list.loadNonCompliances();
        Assert.Equal(2, list.NonCompliancesList.Count);
        Assert.Equal(1, list.NonCompliancesList.Count(n => n.UserID == "admin"));
        Assert.Equal(1, list.NonCompliancesList.Count(n => n.UserID == "gianlucas"));
    }

    [Fact]
    public void NonComplianceTypes_LoadsTwoSeedTypes()
    {
        var types = new NonComplianceTypes(Tenant);
        types.loadTypeList();
        Assert.Equal(2, types.TypeList.Count);
        Assert.Contains(types.TypeList, t => t.Name == "Differenza Ordine-Rilievo");
        Assert.Contains(types.TypeList, t => t.Name == "Documento Rilievo incompleto/poco chiaro");
    }

    [Fact]
    public void NonComplianceTypes_FindIdByName_ResolvesSeedType()
    {
        var types = new NonComplianceTypes(Tenant);
        Assert.Equal(0, types.findIDByName("Differenza Ordine-Rilievo"));
        Assert.Equal(1, types.findIDByName("Documento Rilievo incompleto/poco chiaro"));
        Assert.Equal(-1, types.findIDByName("NoSuchType"));
    }

    [Fact]
    public void NonComplianceTypes_AddThenDelete_RoundTrips()
    {
        var types = new NonComplianceTypes(Tenant);
        var unique = "Type_" + Guid.NewGuid().ToString("N").Substring(0, 8);
        Assert.True(types.Add(unique, "test desc"));
        try
        {
            var id = types.findIDByName(unique);
            Assert.True(id >= 0);
            var loaded = new NonComplianceType(Tenant, id);
            Assert.Equal(unique, loaded.Name);
            Assert.Equal("test desc", loaded.Description);
        }
        finally
        {
            var id = types.findIDByName(unique);
            if (id >= 0) Assert.True(types.Delete(id));
        }
    }

    [Fact]
    public void NonComplianceType_UnknownId_LeavesIdAtMinusOne()
    {
        var t = new NonComplianceType(Tenant, 99999);
        Assert.Equal(-1, t.ID);
        Assert.Equal("", t.Name);
    }

    [Fact]
    public void NonComplianceCauses_LoadsEmptyListFromSeed()
    {
        var causes = new NonComplianceCauses(Tenant);
        causes.loadCausesList();
        Assert.NotNull(causes.CausesList);
        Assert.Empty(causes.CausesList);
    }

    [Fact]
    public void NonComplianceCauses_AddThenDelete_RoundTrips()
    {
        var causes = new NonComplianceCauses(Tenant);
        var unique = "Cause_" + Guid.NewGuid().ToString("N").Substring(0, 8);
        Assert.True(causes.Add(unique, "test desc"));
        try
        {
            var id = causes.findIDByName(unique);
            Assert.True(id >= 0);
            var loaded = new NonComplianceCause(Tenant, id);
            Assert.Equal(unique, loaded.Name);
            Assert.Equal("test desc", loaded.Description);
        }
        finally
        {
            var id = causes.findIDByName(unique);
            if (id >= 0) Assert.True(causes.Delete(id));
        }
    }

    [Fact]
    public void FreeWarnings_LoadsSeedWarnings()
    {
        var fw = new FreeWarnings(Tenant);
        Assert.NotEmpty(fw.FreeWarningList);
        Assert.True(fw.FreeWarningList[0].WarningID > 0);
        Assert.True(fw.FreeWarningList[0].Quantity > 0);
    }

    [Fact]
    public void NonCompliances_AddThenDelete_RoundTrips()
    {
        var ncs = new NonCompliances(Tenant);
        var ret = ncs.Add("admin");
        Assert.True(ret[0] >= 0);
        try
        {
            var nc = new NonCompliance(Tenant, ret[0], ret[1]);
            Assert.Equal(ret[0], nc.ID);
            Assert.Equal("admin", nc.UserID);
        }
        finally
        {
            Assert.True(ncs.Delete(ret[0], ret[1]));
        }
    }

    [Fact]
    public void Postazione_ById_LoadsFromSeed()
    {
        var pst = new Postazione(Tenant, 20);
        Assert.Equal(20, pst.id);
        Assert.Equal("Magazzino", pst.name);
    }

    [Fact]
    public void Postazione_UnknownId_ReturnsMinusOne()
    {
        var pst = new Postazione(Tenant, 99999);
        Assert.Equal(-1, pst.id);
        Assert.Equal("", pst.name);
    }

    [Fact]
    public void ElencoPostazioni_LoadsFromSeed()
    {
        var el = new ElencoPostazioni(Tenant);
        Assert.NotEmpty(el.elenco);
        Assert.Contains(el.elenco, p => p.id == 20);
    }

    [Fact]
    public void Postazione_LoadReparti_LoadsSeed()
    {
        var pst = new Postazione(Tenant, 20);
        Assert.True(pst.loadReparti());
        Assert.NotNull(pst.ElencoIDReparti);
    }

    [Fact]
    public void Postazione_LoadTasks_LoadsProcesses()
    {
        var pst = new Postazione(Tenant, 20);
        pst.loadTasks();
        Assert.NotNull(pst.tasks);
    }

    [Fact]
    public void TaskProduzione_ById_LoadsFromSeed()
    {
        var tsk = new TaskProduzione(Tenant, 95);
        Assert.Equal(95, tsk.TaskProduzioneID);
        Assert.Equal("Taglio Ferro", tsk.Name);
        Assert.Equal(29, tsk.PostazioneID);
        Assert.Equal(2, tsk.RepartoID);
        Assert.Equal('F', tsk.Status);
    }

    [Fact]
    public void TaskProduzione_UnknownId_ReturnsMinusOne()
    {
        var tsk = new TaskProduzione(Tenant, 9999999);
        Assert.Equal(-1, tsk.TaskProduzioneID);
        Assert.Equal("", tsk.Name);
    }

    [Fact]
    public void WorkInstruction_ByIdAndVersion_LoadsFromSeed()
    {
        var wi = new KIS.App_Sources.WorkInstructions.WorkInstruction(Tenant, 0, 0);
        Assert.Equal(0, wi.ID);
        Assert.Equal(0, wi.Version);
        Assert.Equal("Picking", wi.Name);
    }

    [Fact]
    public void WorkInstruction_UnknownId_ReturnsMinusOne()
    {
        var wi = new KIS.App_Sources.WorkInstructions.WorkInstruction(Tenant, 999999, 0);
        Assert.Equal(-1, wi.ID);
        Assert.Equal(-1, wi.Version);
        Assert.Equal("", wi.Name);
    }

    [Fact]
    public void WorkInstruction_LatestVersion_LoadsFromSeed()
    {
        var wi = new KIS.App_Sources.WorkInstructions.WorkInstruction(Tenant, 1);
        Assert.Equal(1, wi.ID);
        Assert.Equal("DDD.pdf", wi.Name);
    }

    [Fact]
    public void ImprovementActions_LoadsEmptyListFromSeed()
    {
        var list = new ImprovementActions(Tenant);
        list.loadImprovementActions();
        Assert.NotNull(list.ImprovementActionsList);
        Assert.Empty(list.ImprovementActionsList);
    }

    [Fact]
    public void InputPoints_EmptyTenant_ListStaysEmpty()
    {
        var ips = new InputPoints("");
        Assert.Equal("", ips.Tenant);
        Assert.Empty(ips.list);
        ips.loadInputPoints();
        Assert.Empty(ips.list);
    }

    [Fact]
    public void InputPoint_EmptyTenant_ReturnsMinusOne()
    {
        var ip = new InputPoint("", 5);
        Assert.Equal(-1, ip.id);
    }

    [Fact]
    public void InputPointDepartment_EmptyTenant_ReturnsMinusOne()
    {
        var d = new InputPointDepartment("", 1, 2);
        Assert.Equal(-1, d.inputpointId);
        Assert.Equal(-1, d.departmentId);
        Assert.Equal(2, d.delete());
    }

    [Fact]
    public void InputPointWorkstation_EmptyTenant_ReturnsMinusOne()
    {
        var w = new InputPointWorkstation("", 1, 2);
        Assert.Equal(-1, w.inputpointId);
        Assert.Equal(-1, w.workstationId);
        Assert.Equal(2, w.delete());
    }

    [Fact]
    public void AvanzamentoProduzioneService_CaricaArticoliNonPianificati_ReturnsOnlyNP()
    {
        var articoli = AvanzamentoProduzioneService.CaricaArticoliNonPianificati(Tenant);
        Assert.NotEmpty(articoli);
        foreach (var art in articoli)
        {
            Assert.True(art.Status == 'N' || art.Status == 'P',
                $"Articolo {art.ID}/{art.Year} ha status '{art.Status}' (atteso N o P)");
        }
        // Seed data contains article 276/2020 (status P).
        Assert.Contains(articoli, a => a.ID == 276 && a.Year == 2020);
    }

    [Fact]
    public void AvanzamentoProduzioneService_ClassificaStato_BeforeAllTasks_IsNonIniziato()
    {
        var art = new Articolo(Tenant, 276, 2020);
        Assert.Equal('P', art.Status);
        var stato = AvanzamentoProduzioneService.ClassificaStato(art, new DateTime(1970, 1, 1));
        Assert.Equal(StatoAvanzamentoArticolo.NonIniziato, stato);
    }

    [Fact]
    public void AvanzamentoProduzioneService_ClassificaStato_AfterAllTasks_IsInRitardo()
    {
        var art = new Articolo(Tenant, 276, 2020);
        Assert.Equal('P', art.Status);
        var stato = AvanzamentoProduzioneService.ClassificaStato(art, new DateTime(2100, 1, 1));
        Assert.Equal(StatoAvanzamentoArticolo.InRitardo, stato);
    }

    [Fact]
    public void AvanzamentoProduzioneService_ClassificaStato_InsideWindow_IsInCorso()
    {
        // Pure classification: a task window straddling the reference instant.
        var finestre = new[]
        {
            (EarlyStart: new DateTime(2020, 2, 10, 8, 0, 0), LateStart: new DateTime(2020, 2, 12, 8, 0, 0))
        };
        var stato = AvanzamentoProduzioneService.ClassificaStato(new DateTime(2020, 2, 11, 12, 0, 0), finestre);
        Assert.Equal(StatoAvanzamentoArticolo.InCorso, stato);
    }

    [Fact]
    public void AvanzamentoProduzioneService_ClassificaStato_LateTaskWinsOverInProgress()
    {
        // Red wins over yellow: one task in progress, one already past LateStart.
        var finestre = new[]
        {
            (EarlyStart: new DateTime(2020, 2, 10, 8, 0, 0), LateStart: new DateTime(2020, 2, 12, 8, 0, 0)),
            (EarlyStart: new DateTime(2020, 1, 1, 0, 0, 0), LateStart: new DateTime(2020, 1, 2, 0, 0, 0))
        };
        var stato = AvanzamentoProduzioneService.ClassificaStato(new DateTime(2020, 2, 11, 12, 0, 0), finestre);
        Assert.Equal(StatoAvanzamentoArticolo.InRitardo, stato);
    }

    [Fact]
    public void PortafoglioClienti_Add_RoundTrips()
    {
        // addCliente.ascx.btnSave_Click path: insert a full customer row
        // (kanbanManaged/customer/provider are bit(1) columns) then read it back.
        const string codice = "DOMTEST_ADD";
        // Cleanup leftovers from previous runs.
        new Cliente(Tenant, codice).Delete();

        var elenco = new PortafoglioClienti(Tenant);
        Assert.True(elenco.Add(codice, "Domain Test RagSoc", "12345678901", "",
            "Via Test 1", "Milano", "MI", "20100", "Italia", "+390212345678",
            "domtest@example.com", kanban: true, provider: true, customer: true),
            "Add fallito: " + elenco.log);

        var ricaricato = new PortafoglioClienti(Tenant).Elenco
            .FirstOrDefault(c => c.CodiceCliente == codice);
        Assert.NotNull(ricaricato);
        Assert.Equal("Domain Test RagSoc", ricaricato!.RagioneSociale);

        Assert.True(new Cliente(Tenant, codice).Delete(), "Delete di pulizia fallito");
    }
}
