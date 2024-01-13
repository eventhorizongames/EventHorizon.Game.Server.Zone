namespace EventHorizon.Zone.System.Server.Scripts.Tests;

using EventHorizon.Game.Server.Zone;
using EventHorizon.Test.Common;
using EventHorizon.Test.Common.Utils;

using FluentAssertions;

using Xunit;

public class SystemServerScriptsPluginSharedExtensionsTests
{
    [Fact]
    public void ShouldRegisterExpectedServices()
    {
        // Given
        var serviceCollectionMock = new ServiceCollectionMock();

        // When
        SystemServerScriptsPluginSharedExtensions.AddSystemServerScriptsPluginShared(
            serviceCollectionMock
        );

        // Then
        serviceCollectionMock
            .Should().BeEmpty();
    }

    [Fact]
    public void ShouldReturnPassedInApplicationBuilderWhenFinishedRunning()
    {
        // Given
        var applicationBuilderMocks = ApplicationBuilderFactory.CreateApplicationBuilder();
        var expected = applicationBuilderMocks.ApplicationBuilderMock.Object;

        // When
        var actual = SystemServerScriptsPluginSharedExtensions.UseSystemServerScriptsPluginShared(
            applicationBuilderMocks.ApplicationBuilderMock.Object
        );

        // Then
        actual
            .Should().Be(expected);
    }
}
