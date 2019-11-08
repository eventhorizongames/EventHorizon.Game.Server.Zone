using Xunit;
using Moq;
using EventHorizon.Game.Server.Zone.Tests.TestUtil;
using EventHorizon.Game.Server.Zone.Setup;
using MediatR;
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using System.Threading;
using EventHorizon.Zone.Core.Events.Map.Create;
using EventHorizon.Game.Server.Zone.Server.Core.Ping.Tasks;
using EventHorizon.TimerService;

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
            ServerSetupExtensions.AddServerSetup(
                serviceCollectionMock
            );

            // Then
            Assert.Collection(
                serviceCollectionMock,
                service =>
                {
                    Assert.Equal(typeof(ITimerTask), service.ServiceType);
                    Assert.Equal(typeof(PingCoreServerTimerTask), service.ImplementationType);
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
            serviceProviderMock.Setup(
                mock => mock.GetService(
                    typeof(IMediator)
                )
            ).Returns(
                mediatorMock.Object
            );
            var serviceScopeMock = new Mock<IServiceScope>();
            serviceScopeMock.SetupGet(
                mock => mock.ServiceProvider
            ).Returns(
                serviceProviderMock.Object
            );
            var serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();
            serviceScopeFactoryMock.Setup(
                mock => mock.CreateScope()
            ).Returns(
                serviceScopeMock.Object
            );
            var applicationServicesMock = new Mock<IServiceProvider>();
            applicationServicesMock.Setup(
                mock => mock.GetService(
                    typeof(IServiceScopeFactory)
                )
            ).Returns(
                serviceScopeFactoryMock.Object
            );

            var applicationBuilderMock = new Mock<IApplicationBuilder>();
            applicationBuilderMock.Setup(
                mock => mock.ApplicationServices
            ).Returns(
                applicationServicesMock.Object
            );

            // When
            ServerSetupExtensions.UseServerSetup(
                applicationBuilderMock.Object
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Publish(
                    expectedLoadZoneAgentStateEvent, 
                    CancellationToken.None
                )
            );
        }
    }
}