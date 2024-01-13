namespace EventHorizon.Zone.System.Wizard.Plugin.Editor.Tests;

using EventHorizon.Game.Server.Zone;
using EventHorizon.Test.Common;
using EventHorizon.Test.Common.Utils;

using FluentAssertions;

using Xunit;

public class SystemWizardPluginEditorExtensionsTests
{
    [Fact]
    public void ShouldReturnPassedInCall()
    {
        // Given
        var serviceCollectionMock = new ServiceCollectionMock();
        var expected = serviceCollectionMock;

        // When
        var actual = SystemWizardPluginEditorExtensions.AddSystemWizardPluginEditor(
            serviceCollectionMock
        );

        // Then
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void ShouldReturnPassedIn()
    {
        // Given
        var applicationBuilder = ApplicationBuilderFactory.CreateApplicationBuilder();
        var expected = applicationBuilder.ApplicationBuilderMock.Object;

        // When
        var actual = SystemWizardPluginEditorExtensions.UseSystemWizardPluginEditor(
            applicationBuilder.ApplicationBuilderMock.Object
        );

        // Then
        actual.Should().Be(expected);
    }
}
