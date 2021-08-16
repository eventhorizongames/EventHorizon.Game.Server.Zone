namespace EventHorizon.Zone.System.Wizard.Tests.Json.Model
{
    using EventHorizon.Zone.System.Wizard.Json.Model;

    using FluentAssertions;

    using global::System.Buffers;
    using global::System.Text;
    using global::System.Text.Json;

    using Xunit;


    public class JsonDataPropertyTests
    {
        [Theory]
        [InlineData("stringProperty", "String", "string-value", "{\"stringProperty\":\"string-value\"}")]
        [InlineData("longProperty", "Long", "1234", "{\"longProperty\":1234}")]
        [InlineData("booleanProperty", "Boolean", "true", "{\"booleanProperty\":true}")]
        public void ShouldCreateExpectedJsonWhenWritingPropertyToWriter(
            string propertyName,
            string propertyType,
            string propertyValue,
            string expected
        )
        {
            // Given

            // When
            var property = new JsonDataProperty(
                propertyName,
                propertyType,
                propertyValue
            );
            var actual = ToJsonString(
                property
            );

            // Then
            actual.Should().Be(expected);
        }

        [Fact]
        public void ShouldCreateComplexObjectWhenPropertyIsObjectType()
        {
            // Given
            var propertyName = "objectProperty";
            var propertyType = "Object";
            var expected = "{\"objectProperty\":{\"objProp1\":\"value1\",\"objProp2\":123}}";

            // When
            var property = new JsonDataProperty(
                propertyName,
                propertyType,
                string.Empty
            );
            property.Data.Add(
                new JsonDataProperty(
                    "objProp1",
                    "String",
                    "value1"
                )
            );
            property.Data.Add(
                new JsonDataProperty(
                    "objProp2",
                    "Long",
                    "123"
                )
            );
            var actual = ToJsonString(
                property
            );

            // Then
            actual.Should().Be(expected);
        }

        public static string ToJsonString(
            JsonDataProperty jsonDataProperty
        )
        {
            var outputBuffer = new ArrayBufferWriter<byte>();
            using (var jsonWriter = new Utf8JsonWriter(
                outputBuffer
            ))
            {
                jsonWriter.WriteStartObject();

                jsonDataProperty.WriteTo(jsonWriter);

                jsonWriter.WriteEndObject();
            }

            return Encoding.UTF8.GetString(
                outputBuffer.WrittenSpan
            );
        }
    }
}
