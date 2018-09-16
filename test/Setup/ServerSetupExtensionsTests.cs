using Xunit;
using Moq;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Tests.TestUtil;
using EventHorizon.Game.Server.Zone.Setup;
using EventHorizon.Game.Server.Zone.State;
using EventHorizon.Game.Server.Zone.State.Impl;
using EventHorizon.Schedule;
using EventHorizon.Game.Server.Zone.Core.Ping;
using EventHorizon.Game.Server.Zone.Map.Create;
using MediatR;
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using System.Threading;

namespace EventHorizon.Game.Server.Zone.Tests.Setup
{
    public class ServerSetupExtensionsTests
    {
        [Fact]
        public void TestAddServerSetup_ShouldAddExpectedServices()
        {
            // Given
            var serviceCollectionMock = new ServiceCollectionMock();

            // When
            ServerSetupExtensions.AddServerSetup(serviceCollectionMock, null);

            // Then
            Assert.NotEmpty(serviceCollectionMock);
            Assert.Collection(serviceCollectionMock,
                service =>
                {
                    Assert.Equal(typeof(IServerState), service.ServiceType);
                    Assert.Equal(typeof(ServerState), service.ImplementationType);
                },
                service =>
                {
                    Assert.Equal(typeof(IScheduledTask), service.ServiceType);
                    Assert.Equal(typeof(PingCoreServerScheduledTask), service.ImplementationType);
                }
            );
        }

        [Fact]
        public void TestUseSetupServer_ShouldSendAndPublishExpectedEvent()
        {
            // Given
            var expectedLoadZoneAgentStateEvent = new CreateMapEvent();

            var mediatorMock = new Mock<IMediator>();

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(serviceProvider => serviceProvider.GetService(typeof(IMediator))).Returns(mediatorMock.Object);
            var serviceScopeMock = new Mock<IServiceScope>();
            serviceScopeMock.SetupGet(serviceScope => serviceScope.ServiceProvider).Returns(serviceProviderMock.Object);
            var serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();
            serviceScopeFactoryMock.Setup(a => a.CreateScope()).Returns(serviceScopeMock.Object);
            var applicationServicesMock = new Mock<IServiceProvider>();
            applicationServicesMock.Setup(a => a.GetService(typeof(IServiceScopeFactory))).Returns(serviceScopeFactoryMock.Object);

            var applicationBuilderMock = new Mock<IApplicationBuilder>();
            applicationBuilderMock.Setup(a => a.ApplicationServices).Returns(applicationServicesMock.Object);

            // When
            ServerSetupExtensions.UseSetupServer(applicationBuilderMock.Object);

            // Then
            mediatorMock.Verify(mediator => mediator.Publish(expectedLoadZoneAgentStateEvent, CancellationToken.None));
        }
    }
}