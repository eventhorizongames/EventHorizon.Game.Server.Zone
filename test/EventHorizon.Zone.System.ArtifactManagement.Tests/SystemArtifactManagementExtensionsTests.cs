namespace EventHorizon.Zone.System.ArtifactManagement.Tests;

using AutoFixture.Xunit2;

using EventHorizon.Game.Server.Zone;
using EventHorizon.Test.Common.Attributes;
using EventHorizon.Test.Common.Utils;

using FluentAssertions;

using global::System.Linq;

using Microsoft.AspNetCore.Builder;

using Moq;

using Xunit;

public class SystemArtifactManagementExtensionsTests
{
    [Theory, AutoMoqData]
    public void TestAddAgent_ShouldConfigureServiceCollection(
        // Given
        ServiceCollectionMock serviceCollectionMock
    )
    {
        // When
        serviceCollectionMock.AddSystemArtifactManagement(
            options => { }
        );

        // Then
        serviceCollectionMock.Should().NotBeEmpty();
        serviceCollectionMock
            .Where(service => service.ServiceType.Equals(typeof(ArtifactManagementSystemSettings)))
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
            .UseSystemArtifactManagement();

        // Then
        actual.Should().Be(
            applicationBuilderMock.Object
        );
    }
}
