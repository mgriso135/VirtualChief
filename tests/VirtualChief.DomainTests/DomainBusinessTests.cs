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
}
