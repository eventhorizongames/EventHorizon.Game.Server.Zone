namespace EventHorizon.Zone.System.EntityModule.Tests;

using EventHorizon.Game.Server.Zone;
using EventHorizon.Test.Common;
using EventHorizon.Test.Common.Utils;
using EventHorizon.Zone.System.EntityModule.Api;
using EventHorizon.Zone.System.EntityModule.Load;
using EventHorizon.Zone.System.EntityModule.State;

using global::System.Threading;

using MediatR;

using Moq;

using Xunit;

public class SystemEntityModuleExtensionsTests
{
    [Fact]
    public void ShouldAddExpectedServices()
    {
        // Given
        var serviceCollectionMock = new ServiceCollectionMock();

        // When
        var actual = SystemEntityModuleExtensions.AddSystemEntityModule(
            serviceCollectionMock
        );

        // Then
        Assert.Collection(
            actual,
            service =>
            {
                Assert.Equal(typeof(EntityModuleRepository), service.ServiceType);
                Assert.Equal(typeof(EntityModuleInMemoryRepository), service.ImplementationType);
            }
        );
    }

    [Fact]
    public void ShouldReturnApplicationBuilderForChainingCommands()
    {
        // Given
        var applicationBuilderMocks = ApplicationBuilderFactory.CreateApplicationBuilder();
        var expected = new LoadEntityModuleSystemCommand();

        var mediatorMock = new Mock<IMediator>();

        applicationBuilderMocks.ServiceProviderMock.Setup(
            mock => mock.GetService(typeof(IMediator))
        ).Returns(
            mediatorMock.Object
        );

        // When
        var actual = SystemEntityModuleExtensions.UseSystemEntityModule(
            applicationBuilderMocks.ApplicationBuilderMock.Object
        );

        // Then
        mediatorMock.Verify(
            mock => mock.Send(
                expected,
                CancellationToken.None
            )
        );
    }
}
