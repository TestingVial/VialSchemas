using System.Text.Json.Nodes;
using Json.Schema;
using Xunit;

namespace VialSchemas.Tests;

/// <summary>The rules that changed in v0.0.2: free-text optional type, required description, no filePath.</summary>
public class SchemaV002Tests
{
    private const string ValidVial = """
        {
          "name": "Cart",
          "description": "Carts can have products added",
          "type": "unit",
          "technology": "csharp",
          "location": { "className": "Shop.Tests.CartTests", "methodName": "AddToCart_AddsProduct" }
        }
        """;

    private static EvaluationResults Evaluate(Action<JsonObject> change)
    {
        var vial = JsonNode.Parse(ValidVial)!.AsObject();
        change(vial);
        var document = new JsonObject { ["schemaVersion"] = "0.0.2", ["vials"] = new JsonArray(vial) };
        return Schemas.Load("0.0.2").Evaluate(document);
    }

    [Fact]
    public void BaseVial_IsValid()
    {
        Assert.True(Evaluate(_ => { }).IsValid);
    }

    [Theory]
    [InlineData("performance")]
    [InlineData("contract")]
    [InlineData("e2e")]
    public void Type_AcceptsAnyNonEmptyString(string type)
    {
        Assert.True(Evaluate(v => v["type"] = type).IsValid);
    }

    [Fact]
    public void Type_IsOptional()
    {
        Assert.True(Evaluate(v => v.Remove("type")).IsValid);
    }

    [Fact]
    public void Technology_IsOptional()
    {
        Assert.True(Evaluate(v => v.Remove("technology")).IsValid);
    }

    [Fact]
    public void Type_RejectsAnEmptyString()
    {
        Assert.False(Evaluate(v => v["type"] = "").IsValid);
    }

    [Fact]
    public void Description_IsRequired()
    {
        Assert.False(Evaluate(v => v.Remove("description")).IsValid);
    }

    [Fact]
    public void Description_RejectsAnEmptyString()
    {
        Assert.False(Evaluate(v => v["description"] = "").IsValid);
    }

    [Fact]
    public void SourceLocation_RejectsFilePath()
    {
        Assert.False(Evaluate(v => v["location"]!["filePath"] = "tests/CartTests.cs").IsValid);
    }

    [Theory]
    [InlineData("className")]
    [InlineData("methodName")]
    public void SourceLocation_RequiresClassAndMethod(string property)
    {
        Assert.False(Evaluate(v => v["location"]!.AsObject().Remove(property)).IsValid);
    }

    [Fact]
    public void UrlLocation_IsValid()
    {
        Assert.True(Evaluate(v => v["location"] = new JsonObject { ["url"] = "https://example.test/checkout" }).IsValid);
    }
}
