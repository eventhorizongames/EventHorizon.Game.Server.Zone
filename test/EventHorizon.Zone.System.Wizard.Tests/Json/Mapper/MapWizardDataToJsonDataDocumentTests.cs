namespace EventHorizon.Zone.System.Wizard.Tests.Json.Mapper
{
    using EventHorizon.Zone.System.Wizard.Json.Mapper;
    using EventHorizon.Zone.System.Wizard.Model;

    using FluentAssertions;

    using global::System.Collections.Generic;
    using global::System.Linq;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using Xunit;


    public class MapWizardDataToJsonDataDocumentTests
    {
        [Fact]
        public async Task ShouldReturnEmptyJsonDataDocumentWhenWizardDataContainsZeroProperties()
        {
            // Given
            var wizardData = new WizardData(
                new Dictionary<string, string>()
            );

            // When
            var handler = new MapWizardDataToJsonDataDocumentHandler();
            var actual = await handler.Handle(
                new MapWizardDataToJsonDataDocument(
                    wizardData
                ),
                CancellationToken.None
            );

            // Then
            actual.Properties
                .Should().BeEmpty();
        }

        [Fact]
        public async Task ShouldAddPropertyToJsonDataDocumentWhenFoundInCorrectFormatInWizardData()
        {
            // Given
            var wizardData = new WizardData(
                new Dictionary<string, string>
                {
                    ["property:dimensions"] = "Long",
                    ["dimensions"] = "412",
                    ["property:tileDimensions"] = "Long",
                    ["tileDimensions"] = "1",
                }
            );

            var expectedProperty1Name = "dimensions";
            var expectedProperty1Type = "Long";
            var expectedProperty1Value = "412";
            var expectedProperty2Name = "tileDimensions";
            var expectedProperty2Type = "Long";
            var expectedProperty2Value = "1";

            // When
            var handler = new MapWizardDataToJsonDataDocumentHandler();
            var actual = await handler.Handle(
                new MapWizardDataToJsonDataDocument(
                    wizardData
                ),
                CancellationToken.None
            );

            // Then
            var actualProperty1 = actual.Properties.FirstOrDefault(
                a => a.Name == expectedProperty1Name
            );
            actualProperty1.Should().NotBeNull();
            actualProperty1.Name
                .Should().Be(expectedProperty1Name);
            actualProperty1.Type
                .Should().Be(expectedProperty1Type);
            actualProperty1.Value
                .Should().Be(expectedProperty1Value);
            actualProperty1.Data
                .Should().BeEmpty();

            var actualProperty2 = actual.Properties.FirstOrDefault(
                a => a.Name == expectedProperty2Name
            );
            actualProperty2.Should().NotBeNull();
            actualProperty2.Name
                .Should().Be(expectedProperty2Name);
            actualProperty2.Type
                .Should().Be(expectedProperty2Type);
            actualProperty2.Value
                .Should().Be(expectedProperty2Value);
            actualProperty2.Data
                .Should().BeEmpty();
        }

        [Fact]
        public async Task ShouldAddPropertyToParentWhenPropertyIsFormattingWithParentProperty()
        {
            // Given
            var wizardData = new WizardData(
                new Dictionary<string, string>
                {
                    ["property:mesh:dimensions"] = "Long",
                    ["mesh:dimensions"] = "412",
                }
            );
            var parentPropertyName = "mesh";

            var expectedPropertyName = "dimensions";
            var expectedPropertyType = "Long";
            var expectedPropertyValue = "412";

            // When
            var handler = new MapWizardDataToJsonDataDocumentHandler();
            var actual = await handler.Handle(
                new MapWizardDataToJsonDataDocument(
                    wizardData
                ),
                CancellationToken.None
            );

            // Then
            var actualParentProperty = actual.Properties.FirstOrDefault(
                a => a.Name == parentPropertyName
            );
            actualParentProperty.Should().NotBeNull();

            var actualProperty = actualParentProperty.Data.FirstOrDefault(
                a => a.Name == expectedPropertyName
            );
            actualProperty.Should().NotBeNull();
            actualProperty.Name
                .Should().Be(expectedPropertyName);
            actualProperty.Type
                .Should().Be(expectedPropertyType);
            actualProperty.Value
                .Should().Be(expectedPropertyValue);
        }

        [Fact]
        public async Task ShouldCorrectlyFillSubParentPropertiesWhenParentsAreMultipleLayersDeep()
        {
            // Given
            var wizardData = new WizardData(
                new Dictionary<string, string>
                {
                    ["property:mesh:material:dimensions"] = "String",
                    ["mesh:material:dimensions"] = "String value Is a String",
                }
            );
            var parentPropertyName = "material";
            var parentParentPropertyName = "mesh";

            var expectedPropertyName = "dimensions";
            var expectedPropertyType = "String";
            var expectedPropertyValue = "String value Is a String";

            // When
            var handler = new MapWizardDataToJsonDataDocumentHandler();
            var actual = await handler.Handle(
                new MapWizardDataToJsonDataDocument(
                    wizardData
                ),
                CancellationToken.None
            );

            // Then
            var actualParentParentProperty = actual.Properties.FirstOrDefault(
                a => a.Name == parentParentPropertyName
            );
            actualParentParentProperty.Should().NotBeNull();

            var actualParentProperty = actualParentParentProperty.Data.FirstOrDefault(
                a => a.Name == parentPropertyName
            );
            actualParentProperty.Should().NotBeNull();

            var actualProperty = actualParentProperty.Data.FirstOrDefault(
                a => a.Name == expectedPropertyName
            );
            actualProperty.Should().NotBeNull();
            actualProperty.Name
                .Should().Be(expectedPropertyName);
            actualProperty.Type
                .Should().Be(expectedPropertyType);
            actualProperty.Value
                .Should().Be(expectedPropertyValue);
        }
    }
}

