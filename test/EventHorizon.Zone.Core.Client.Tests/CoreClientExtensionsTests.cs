namespace EventHorizon.Zone.Core.Client.Tests;

using EventHorizon.Game.Server.Zone;
using EventHorizon.Test.Common.Utils;

using Xunit;

public class CoreClientExtensionsTests
{
    [Fact]
    public void TestShouldConfigurationServiceCollection()
    {
        // Given
        var serviceCollectionMock = new ServiceCollectionMock();

        // When
        CoreClientExtensions.AddCoreClient(
            serviceCollectionMock
        );

        // Then
        Assert.Empty(
            serviceCollectionMock
        );
    }
}
