using System.Text.Json.Nodes;
using Xunit;

namespace VialSchemas.Tests;

/// <summary>
/// Keeps versions.json, the files in the "schemas" folder and each schema's "$id" in agreement, so the published
/// URLs always resolve to the schema they name.
/// </summary>
public class VersionsIndexTests
{
    private const string RawBaseUrl = "https://raw.githubusercontent.com/TestingVial/VialSchemas/main/schemas/";

    private static IReadOnlyList<(string Version, string Url)> IndexEntries()
    {
        var index = JsonNode.Parse(File.ReadAllText(Path.Combine(RepoPaths.RepoRoot, "versions.json")));
        return index!["versions"]!.AsArray()
            .Select(entry => (entry!["version"]!.GetValue<string>(), entry["url"]!.GetValue<string>()))
            .ToList();
    }

    private static IEnumerable<string> SchemaFileNames() =>
        Directory.EnumerateFiles(RepoPaths.SchemasDir, "*.json").Select(path => Path.GetFileName(path)!);

    public static IEnumerable<object[]> SchemaFiles() =>
        SchemaFileNames().Order(StringComparer.Ordinal).Select(name => new object[] { name });

    [Fact]
    public void Index_ListsEverySchemaFile_AndNothingElse()
    {
        Assert.Equal(
            SchemaFileNames().Order(StringComparer.Ordinal),
            IndexEntries().Select(e => $"{e.Version}.json").Order(StringComparer.Ordinal));
    }

    [Fact]
    public void Index_Urls_PointAtTheSchemaFileOfTheirVersion()
    {
        Assert.All(IndexEntries(), entry => Assert.Equal($"{RawBaseUrl}{entry.Version}.json", entry.Url));
    }

    [Theory]
    [MemberData(nameof(SchemaFiles))]
    public void Schema_Id_IsItsRawUrl(string fileName)
    {
        var schema = JsonNode.Parse(File.ReadAllText(Path.Combine(RepoPaths.SchemasDir, fileName)));

        Assert.Equal($"{RawBaseUrl}{fileName}", schema!["$id"]?.GetValue<string>());
    }

    [Theory]
    [MemberData(nameof(SchemaFiles))]
    public void Schema_SchemaVersionConst_MatchesItsFileName(string fileName)
    {
        var schema = JsonNode.Parse(File.ReadAllText(Path.Combine(RepoPaths.SchemasDir, fileName)));

        Assert.Equal(
            Path.GetFileNameWithoutExtension(fileName).TrimStart('v'),
            schema!["properties"]?["schemaVersion"]?["const"]?.GetValue<string>());
    }
}
