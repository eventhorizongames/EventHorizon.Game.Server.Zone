namespace EventHorizon.Zone.System.AssetServer.Tests;

using AutoFixture.Xunit2;

using EventHorizon.Game.Server.Zone;
using EventHorizon.Test.Common.Attributes;
using EventHorizon.Test.Common.Utils;

using FluentAssertions;

using global::System.Linq;

using Microsoft.AspNetCore.Builder;

using Moq;

using Xunit;

public class SystemAssetServerExtensionsTests
{
    [Theory, AutoMoqData]
    public void ConfiuresServiceCollectionWhenExtensionMethodIsCalledForAssetServer(
        // Given
        ServiceCollectionMock serviceCollectionMock
    )
    {
        // When
        serviceCollectionMock.AddSystemAssetServer(
            options => { }
        );

        // Then
        serviceCollectionMock.Should().NotBeEmpty();
        serviceCollectionMock
            .Where(service => service.ServiceType.Equals(typeof(AssetServerSystemSettings)))
            .Should()
            .NotBeEmpty();
    }

    [Theory, AutoMoqData]
    public void AllowForChaninigWhenApplicationBuilderExtensionCalled(
        // Given
        [Frozen] Mock<IApplicationBuilder> applicationBuilderMock
    )
    {
        // When
        var actual = applicationBuilderMock.Object
            .UseSystemAssetServer();

        // Then
        actual.Should().Be(
            applicationBuilderMock.Object
        );
    }
}
