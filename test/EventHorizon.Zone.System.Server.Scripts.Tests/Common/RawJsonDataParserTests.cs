namespace EventHorizon.Zone.System.Server.Scripts.Tests.Common;

using EventHorizon.Zone.System.Server.Scripts.Parsers;
using FluentAssertions;
using global::System.Collections.Generic;
using global::System.Linq;
using global::System.Text.Json;
using Xunit;

public class RawJsonDataParserTests
{
    [Fact]
    public void ShouldReturnEmptyDictionaryWhenJsonIsEmpty()
    {
        // Given
        var jsonDocument = JsonDocument.Parse("{}");

        // When
        var actual = RawJsonDataParser.Parse(jsonDocument);

        // Then
        actual.Should().BeEmpty();
    }

    [Fact]
    public void ShouldReturnDictionaryOfStringsWhenJsonHasStringValues()
    {
        // Given
        var jsonDocument = JsonDocument.Parse("{\"key\":\"value\"}");

        // When
        var actual = RawJsonDataParser.Parse(jsonDocument);

        // Then
        actual.Should().BeEquivalentTo(new Dictionary<string, string> { ["key"] = "value" });
    }

    [Fact]
    public void ShouldReturnDictionaryOfStringsWhenJsonHasNumberValues()
    {
        // Given
        var jsonDocument = JsonDocument.Parse("{\"key\":123}");

        // When
        var actual = RawJsonDataParser.Parse(jsonDocument);

        // Then
        actual.Should().BeEquivalentTo(new Dictionary<string, string> { ["key"] = "123" });
    }

    [Fact]
    public void ShouldReturnDictionaryOfStringsWhenJsonHasBooleanValues()
    {
        // Given
        var jsonDocument = JsonDocument.Parse("{\"key\":true}");

        // When
        var actual = RawJsonDataParser.Parse(jsonDocument);

        // Then
        actual.Should().BeEquivalentTo(new Dictionary<string, string> { ["key"] = "true" });
    }

    [Fact]
    public void ShouldReturnDictionaryOfStringsWhenJsonHasArrayValues()
    {
        // Given
        var jsonDocument = JsonDocument.Parse("{\"key\":[\"value\"]}");

        // When
        var actual = RawJsonDataParser.Parse(jsonDocument);

        // Then
        actual.Should().BeEquivalentTo(new Dictionary<string, string> { ["key:[0]"] = "value" });
    }

    [Fact]
    public void ShouldReturnDictionaryOfStringsWhenJsonHasObjectValues()
    {
        // Given
        var jsonDocument = JsonDocument.Parse("{\"key\":{\"child\":\"value\"}}");

        // When
        var actual = RawJsonDataParser.Parse(jsonDocument);

        // Then
        actual.Should().BeEquivalentTo(new Dictionary<string, string> { ["key:child"] = "value" });
    }

    [Fact]
    public void ShouldReturnDictionaryOfStringsWhenJsonHasNestedObjectValues()
    {
        // Given
        var jsonDocument = JsonDocument.Parse("{\"key\":{\"child\":{\"nested\":\"value\"}}}");

        // When
        var actual = RawJsonDataParser.Parse(jsonDocument);

        // Then
        actual
            .Should()
            .BeEquivalentTo(new Dictionary<string, string> { ["key:child:nested"] = "value" });
    }

    [Fact]
    public void ShouldReturnDictionaryOfStringsWhenJsonHasNestedArrayValues()
    {
        // Given
        var jsonDocument = JsonDocument.Parse("{\"key\":[{\"nested\":\"value\"}]}");

        // When
        var actual = RawJsonDataParser.Parse(jsonDocument);

        // Then
        actual
            .Should()
            .BeEquivalentTo(new Dictionary<string, string> { ["key:[0]:nested"] = "value" });
    }

    [Fact]
    public void ShouldReturnDictionaryOfStringsWhenJsonHasNestedArrayAndObjectValues()
    {
        // Given
        var jsonDocument = JsonDocument.Parse(
            "{\"key\":[{\"nested\":\"value\"},{\"nested\":\"value\"}]}"
        );

        // When
        var actual = RawJsonDataParser.Parse(jsonDocument);

        // Then
        actual
            .Should()
            .BeEquivalentTo(
                new Dictionary<string, string>
                {
                    ["key:[0]:nested"] = "value",
                    ["key:[1]:nested"] = "value",
                }
            );
    }

    [Fact]
    public void ShouldReturnDictionaryOfStringsWhenJsonHasNestedArrayAndObjectValuesWithNestedObject()
    {
        // Given
        var jsonDocument = JsonDocument.Parse(
            "{\"key\":[{\"nested\":\"value\"},{\"nested\":{\"child\":\"value\"}}]}"
        );

        // When
        var actual = RawJsonDataParser.Parse(jsonDocument);

        // Then
        actual
            .Should()
            .BeEquivalentTo(
                new Dictionary<string, string>
                {
                    ["key:[0]:nested"] = "value",
                    ["key:[1]:nested:child"] = "value",
                }
            );
    }

    [Fact]
    public void ShouldReturnDictionaryOfStringsWhenJsonHasNestedArrayAndObjectValuesWithNestedArray()
    {
        // Given
        var jsonDocument = JsonDocument.Parse(
            "{\"key\":[{\"nested\":\"value\"},{\"nested\":[\"value\"]}]}"
        );

        // When
        var actual = RawJsonDataParser.Parse(jsonDocument);

        // Then
        actual
            .Should()
            .BeEquivalentTo(
                new Dictionary<string, string>
                {
                    ["key:[0]:nested"] = "value",
                    ["key:[1]:nested:[0]"] = "value",
                }
            );
    }
}
