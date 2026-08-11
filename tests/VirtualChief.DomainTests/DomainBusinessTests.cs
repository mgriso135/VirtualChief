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
}
