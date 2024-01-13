namespace EventHorizon.Zone.System.Editor.Tests;

using EventHorizon.Game.Server.Zone;
using EventHorizon.Test.Common;
using EventHorizon.Test.Common.Utils;

using FluentAssertions;

using Microsoft.AspNetCore.Builder;

using Xunit;

public class SystemEditorExtensionsTests
{
    [Fact]
    public void TestAddAgent_ShouldConfigureServiceCollection()
    {
        // Given
        var serviceCollectionMock = new ServiceCollectionMock();

        // When
        SystemEditorExtensions.AddSystemEditor(
            serviceCollectionMock
        );

        // Then
        serviceCollectionMock.Services
            .Should().BeEmpty();
    }

    [Fact]
    public void ShouldSendLoadSystemClientEntitiesCommand()
    {
        // Given
        var mocks = ApplicationBuilderFactory.CreateApplicationBuilder();
        var expected = mocks.ApplicationBuilderMock.Object;

        // When
        var actual = SystemEditorExtensions.UseSystemEditor(
            mocks.ApplicationBuilderMock.Object
        );

        // Then
        Assert.Equal(
            expected,
            actual
        );
    }
}
