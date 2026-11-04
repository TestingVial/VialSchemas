using System.Text.Json;
using System.Text.Json.Nodes;
using Json.Schema;
using Xunit;

namespace VialSchemas.Tests;

/// <summary>
/// Validates every file in the "examples" folder is valid JSON and conforms to the JSON schema
/// referenced by its "schemaVersion" property. New files added to "examples" are picked up
/// automatically because test cases are discovered at run time.
/// </summary>
public class ExampleSchemaValidationTests
{
    public static IEnumerable<object[]> ExampleFiles()
    {
        return Directory.EnumerateFiles(RepoPaths.ExamplesDir, "*", SearchOption.AllDirectories)
            .OrderBy(path => path, StringComparer.Ordinal)
            .Select(path => new object[] { path });
    }

    [Theory]
    [MemberData(nameof(ExampleFiles))]
    public void Example_Is_Valid_Json(string examplePath)
    {
        var text = File.ReadAllText(examplePath);

        var exception = Record.Exception(() => JsonNode.Parse(text));

        Assert.True(exception is null, $"{Path.GetFileName(examplePath)} is not valid JSON: {exception?.Message}");
    }

    [Theory]
    [MemberData(nameof(ExampleFiles))]
    public void Example_Matches_Referenced_Schema(string examplePath)
    {
        var text = File.ReadAllText(examplePath);
        var instance = JsonNode.Parse(text);
        Assert.NotNull(instance);

        var schemaVersion = instance!["schemaVersion"]?.GetValue<string>();
        Assert.False(string.IsNullOrWhiteSpace(schemaVersion),
            $"{Path.GetFileName(examplePath)} is missing a \"schemaVersion\" property.");

        var schemaPath = Schemas.PathOf(schemaVersion!);
        Assert.True(File.Exists(schemaPath),
            $"{Path.GetFileName(examplePath)} references schemaVersion \"{schemaVersion}\", but {Path.GetFileName(schemaPath)} was not found in the schemas folder.");

        var results = Schemas.Load(schemaVersion!).Evaluate(instance, new EvaluationOptions { OutputFormat = OutputFormat.List });

        Assert.True(results.IsValid,
            $"{Path.GetFileName(examplePath)} does not conform to {Path.GetFileName(schemaPath)}:\n" +
            string.Join('\n', results.Details.Where(d => !d.IsValid).Select(d => $"  - {d.EvaluationPath}: {string.Join(", ", d.Errors?.Values ?? Enumerable.Empty<string>())}")));
    }
}
