namespace EventHorizon.Server.Core.Tests;

using System;
using System.Threading;

using EventHorizon.Server.Core.Connection;
using EventHorizon.Server.Core.Connection.Internal;
using EventHorizon.Server.Core.Connection.Model;
using EventHorizon.Server.Core.Events.Register;
using EventHorizon.Server.Core.State;
using EventHorizon.Server.Core.Timer;
using EventHorizon.Test.Common;
using EventHorizon.Test.Common.Utils;
using EventHorizon.TimerService;

using MediatR;

using Microsoft.Extensions.Options;

using Moq;

using Xunit;

public class ServerCoreExtensionsTests
{
    [Fact]
    public void TestShouldConfigureServiceCollectionWithExpectedServices()
    {
        // Given
        var coreServerConnectionMock = new Mock<CoreServerConnection>();
        var expectedCoreServerConnection = coreServerConnectionMock.Object;

        static void configureCoreSettings(CoreSettings options) { }

        var serviceCollectionMock = new ServiceCollectionMock();

        var serviceProviderMock = new Mock<IServiceProvider>();
        var coreServerConnectionFactoryMock = new Mock<CoreServerConnectionFactory>();

        serviceProviderMock.Setup(
            mock => mock.GetService(
                typeof(CoreServerConnectionFactory)
            )
        ).Returns(
            coreServerConnectionFactoryMock.Object
        );

        coreServerConnectionFactoryMock.Setup(
            mock => mock.GetConnection()
        ).ReturnsAsync(
            coreServerConnectionMock.Object
        );

        // When
        ServerCoreExtensions.AddServerCore(
            serviceCollectionMock,
            configureCoreSettings
        );

        // Then
        Assert.Contains(
            serviceCollectionMock,
            service =>
                service.ServiceType == typeof(ITimerTask)
                &&
                service.ImplementationType == typeof(PingCoreServerTimerTask)
        );
        Assert.Contains(
            serviceCollectionMock,
            service =>
                service.ServiceType == typeof(ServerCoreCheckState)
                &&
                service.ImplementationType == typeof(SystemServerCoreCheckState)
        );
        Assert.Contains(
            serviceCollectionMock,
            service =>
                service.ServiceType == typeof(CoreServerConnectionCache)
                &&
                service.ImplementationType == typeof(SystemCoreServerConnectionCache)
        );
        Assert.Contains(
            serviceCollectionMock,
            service =>
                service.ServiceType == typeof(CoreServerConnectionFactory)
                &&
                service.ImplementationType == typeof(SystemCoreServerConnectionFactory)
        );
        Assert.Contains(
            serviceCollectionMock,
            service =>
                service.ServiceType == typeof(IConfigureOptions<CoreSettings>)
        );
    }

    [Fact]
    public void ShouldReturnApplicationBuilderForChanning()
    {
        // Given
        var mocks = ApplicationBuilderFactory.CreateApplicationBuilder();
        var expected = mocks.ApplicationBuilderMock.Object;

        // When
        var actual = ServerCoreExtensions.UseServerCore(
            mocks.ApplicationBuilderMock.Object
        );

        // Then
        Assert.Equal(
            expected,
            actual
        );
    }

    [Fact]
    public void ShouldPublishEventsWhenCalled()
    {
        // Given
        var expected = new RegisterWithCoreServer();
        var mocks = ApplicationBuilderFactory.CreateApplicationBuilder();

        var mediatorMock = new Mock<IMediator>();

        mocks.ServiceProviderMock.Setup(
            mock => mock.GetService(
                typeof(IMediator)
            )
        ).Returns(
            mediatorMock.Object
        );

        // When
        var actual = ServerCoreExtensions.UseRegisterWithCoreServer(
            mocks.ApplicationBuilderMock.Object
        );

        // Then
        mediatorMock.Verify(
            mock => mock.Publish(
                expected,
                CancellationToken.None
            )
        );
    }
}
