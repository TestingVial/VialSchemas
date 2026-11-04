using System.Collections.Concurrent;
using Json.Schema;

namespace VialSchemas.Tests;

/// <summary>Loads the schema files of the "schemas" folder.</summary>
internal static class Schemas
{
    // Building a schema registers its $id globally, so each schema is built once per test run.
    private static readonly ConcurrentDictionary<string, Lazy<JsonSchema>> Cache = new();

    public static string PathOf(string schemaVersion) => Path.Combine(RepoPaths.SchemasDir, $"v{schemaVersion}.json");

    public static JsonSchema Load(string schemaVersion) =>
        Cache.GetOrAdd(schemaVersion, v => new Lazy<JsonSchema>(() => JsonSchema.FromText(File.ReadAllText(PathOf(v))))).Value;
}
