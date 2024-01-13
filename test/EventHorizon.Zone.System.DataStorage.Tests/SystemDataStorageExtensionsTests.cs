namespace EventHorizon.Zone.System.DataStorage.Tests;

using EventHorizon.Game.Server.Zone;
using EventHorizon.Test.Common;
using EventHorizon.Test.Common.Utils;
using EventHorizon.TimerService;
using EventHorizon.Zone.System.DataStorage.Api;
using EventHorizon.Zone.System.DataStorage.Load;
using EventHorizon.Zone.System.DataStorage.Model;
using EventHorizon.Zone.System.DataStorage.Provider;
using EventHorizon.Zone.System.DataStorage.Timer;

using global::System;
using global::System.Threading;

using MediatR;

using Moq;

using Xunit;

public class SystemDataStorageExtensionsTests
{
    [Fact]
    public void TestAddServerSetup_ShouldAddExpectedServices()
    {
        // Given
        var serviceCollectionMock = new ServiceCollectionMock();
        var serviceProviderMock = new Mock<IServiceProvider>();

        var standardDataStoreProviderMock = new Mock<StandardDataStoreProvider>();
        serviceProviderMock.Setup(
            mock => mock.GetService(
                typeof(StandardDataStoreProvider)
            )
        ).Returns(
            standardDataStoreProviderMock.Object
        );
        // When
        SystemDataStorageExtensions.AddSystemDataStorage(
            serviceCollectionMock
        );

        // Then
        Assert.Collection(
            serviceCollectionMock,
            service =>
            {
                Assert.Equal(typeof(StandardDataStoreProvider), service.ServiceType);
                Assert.Equal(typeof(StandardDataStoreProvider), service.ImplementationType);
            },
            service =>
            {
                Assert.Equal(typeof(DataStore), service.ServiceType);
                Assert.True(service.ImplementationFactory(serviceProviderMock.Object) is StandardDataStoreProvider);
            },
            service =>
            {
                Assert.Equal(typeof(DataStoreManagement), service.ServiceType);
                Assert.True(service.ImplementationFactory(serviceProviderMock.Object) is StandardDataStoreProvider);
            },
            service =>
            {
                Assert.Equal(typeof(ITimerTask), service.ServiceType);
                Assert.Equal(typeof(SaveDataStoreTimerTask), service.ImplementationType);
            }
        );
    }

    [Fact]
    public void TestShouldReturnApplicationBuilderForChainingCommands()
    {
        // Given
        var applicationBuilderMocks = ApplicationBuilderFactory.CreateApplicationBuilder();
        var expected = new LoadDataStoreCommand();

        var mediatorMock = new Mock<IMediator>();

        applicationBuilderMocks.ServiceProviderMock.Setup(
            mock => mock.GetService(typeof(IMediator))
        ).Returns(
            mediatorMock.Object
        );
        // When
        var actual = SystemDataStorageExtensions.UseSystemDataStorage(
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
