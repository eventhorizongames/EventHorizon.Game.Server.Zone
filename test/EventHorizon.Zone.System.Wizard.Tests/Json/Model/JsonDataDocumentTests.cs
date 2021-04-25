namespace EventHorizon.Zone.System.Wizard.Tests.Json.Model
{
    using EventHorizon.Zone.System.Wizard.Json.Model;
    using FluentAssertions;
    using global::System.Collections.Generic;
    using Xunit;


    public class JsonDataDocumentTests
    {
        [Fact]
        public void ShouldReturnEmptyJsonObjectWhenContainsNullPropertiesList()
        {
            // Given
            var expected = "{}";

            // When
            var document = new JsonDataDocument(
                null
            );
            var actual = document.ToJsonString();

            // Then
            actual.Should().Be(expected);
        }

        [Fact]
        public void ShouldReturnEmptyJsonObjectWhenContainsEmptyPropertiesList()
        {
            // Given
            var expected = "{}";

            // When
            var document = new JsonDataDocument(
                new List<JsonDataProperty>()
            );
            var actual = document.ToJsonString();

            // Then
            actual.Should().Be(expected);
        }

        [Fact]
        public void ShouldReturnEmptyJsonObjectWhenDefaultJsonDataDocumentIsCreated()
        {
            // Given
            var expected = "{}";

            // When
            var document = default(JsonDataDocument);
            var actual = document.ToJsonString();

            // Then
            actual.Should().Be(expected);
        }

        [Fact]
        public void ShouldReturnJsonStringWithPropertiesWhenPropertyListIsNotEmpty()
        {
            // Given
            var propertyList = new List<JsonDataProperty>
            {
                new JsonDataProperty(
                    "property-1",
                    "String",
                    "hello"
                ),
            };
            var expected = "{\"property-1\":\"hello\"}";

            // When
            var document = new JsonDataDocument(
                propertyList
            );
            var actual = document.ToJsonString();


            // Then
            actual.Should().Be(expected);
        }
    }
}
