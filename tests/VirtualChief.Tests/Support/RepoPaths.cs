using System;
using System.IO;

namespace VirtualChief.Tests.Support;

/// <summary>
/// Locates the repository root ("Virtual Chief.sln") from the test assembly directory.
/// </summary>
public static class RepoPaths
{
    public static string Root { get; } = FindRoot();

    static string FindRoot()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir != null)
        {
            if (File.Exists(Path.Combine(dir.FullName, "Virtual Chief.sln")))
                return dir.FullName;
            dir = dir.Parent;
        }
        throw new InvalidOperationException("Cannot locate repository root (Virtual Chief.sln not found walking up).");
    }

    public static string KisWebApp => Path.Combine(Root, "KisWebApp");
    public static string WebConfig => Path.Combine(KisWebApp, "Web.config");
}
