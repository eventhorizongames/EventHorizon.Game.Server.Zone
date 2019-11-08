using Xunit;
using Moq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using EventHorizon.Zone.System.Agent.Save;
using Microsoft.AspNetCore.Builder;
using System;
using MediatR;
using System.Threading;
using EventHorizon.Zone.System.Agent.Model;
using EventHorizon.Zone.System.Agent.Connection;
using EventHorizon.Zone.System.Agent.Connection.Factory;
using EventHorizon.Zone.System.Agent.Model.State;
using EventHorizon.Zone.System.Agent.Events.Startup;
using EventHorizon.Tests.TestUtils;
using EventHorizon.Zone.System.Agent.State;
using EventHorizon.TimerService;

namespace EventHorizon.Game.Server.Zone.Tests.Agent
{
    public class AgentExtensionsTests
    {
        [Fact]
        public void TestAddAgent_ShouldConfigureServiceCollection()
        {
            // Given
            var serviceCollectionMock = new ServiceCollectionMock();
            var configurationMock = new Mock<IConfiguration>();
            var config = new ConfigurationSection(
                new ConfigurationRoot(
                    new List<IConfigurationProvider>()
                ),
                "Agent"
            );
            configurationMock.Setup(
                mock => mock.GetSection(
                    "Agent"
                )
            ).Returns(
                config
            );
            new AgentSettings();

            // When
            SystemAgentExtensions.AddSystemAgent(
                serviceCollectionMock,
                configurationMock.Object
            );

            // Then
            Assert.NotEmpty(
                serviceCollectionMock
            );
            Assert.Contains(
                serviceCollectionMock.Services,
                service =>
                {
                    return typeof(IAgentRepository) == service.Value.ServiceType
                        && typeof(AgentWrappedEntityRepository) == service.Value.ImplementationType;
                }
            );
            Assert.Contains(
                serviceCollectionMock.Services,
                service =>
                {
                    return typeof(ITimerTask) == service.Value.ServiceType
                        && typeof(SaveAgentStateTimerTask) == service.Value.ImplementationType;
                }
            );
            Assert.Contains(
                serviceCollectionMock.Services,
                service =>
                {
                    return typeof(IAgentConnectionCache) == service.Value.ServiceType
                        && typeof(AgentConnectionCache) == service.Value.ImplementationType;
                }
            );
            Assert.Contains(
                serviceCollectionMock.Services,
                service =>
                {
                    return typeof(IAgentConnectionFactory) == service.Value.ServiceType
                        && typeof(AgentConnectionFactory) == service.Value.ImplementationType;
                }
            );
        }
        [Fact]
        public void TestUseAgent_ShouldSendAndPublishExpectedEvent()
        {
            // Given
            var expectedLoadZoneAgentStateEvent = new LoadZoneAgentStateEvent();

            var mediatorMock = new Mock<IMediator>();

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(
                serviceProvider => serviceProvider.GetService(
                    typeof(IMediator)
                )
            ).Returns(
                mediatorMock.Object
            );

            var serviceScopeMock = new Mock<IServiceScope>();
            serviceScopeMock.SetupGet(
                serviceScope => serviceScope.ServiceProvider
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
            SystemAgentExtensions.UseSystemAgent(
                applicationBuilderMock.Object
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Send(
                    expectedLoadZoneAgentStateEvent,
                    CancellationToken.None
                )
            );
        }
    }
}