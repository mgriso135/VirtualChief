using System.Runtime.CompilerServices;

namespace VirtualChief.DomainTests;

/// <summary>
/// The xunit testhost does not automatically load the test assembly's App.config,
/// so System.Configuration.ConfigurationManager sees no connection strings.
/// Point it at the copied <AssemblyName>.dll.config before any business code runs.
/// </summary>
internal static class TestConfig
{
    [ModuleInitializer]
    internal static void Init()
    {
        var cfg = Path.Combine(AppContext.BaseDirectory, "VirtualChief.DomainTests.dll.config");
        if (File.Exists(cfg))
        {
            AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", cfg);
        }
    }
}
