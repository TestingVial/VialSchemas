using System.Text.Json.Nodes;

namespace VialSchemas.Tests;

/// <summary>Locates repository root by walking up from the test binary until versions.json is found.</summary>
internal static class RepoPaths
{
    public static string RepoRoot { get; } = FindRepoRoot();

    public static string ExamplesDir => Path.Combine(RepoRoot, "examples");

    public static string SchemasDir => Path.Combine(RepoRoot, "schemas");

    private static string FindRepoRoot()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir is not null && !File.Exists(Path.Combine(dir.FullName, "versions.json")))
        {
            dir = dir.Parent;
        }

        return dir?.FullName
            ?? throw new InvalidOperationException("Could not locate repository root (versions.json not found).");
    }
}
