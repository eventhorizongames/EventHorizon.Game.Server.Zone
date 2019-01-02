using Xunit;
using Moq;
using Microsoft.Extensions.DependencyInjection;
using EventHorizon.Game.Server.Zone.Agent;
using Microsoft.Extensions.Configuration;
using System.Collections;
using System.Collections.Generic;
using EventHorizon.Game.Server.Zone.State.Repository;
using EventHorizon.Game.Server.Zone.Agent.Move.Repository;
using EventHorizon.Game.Server.Zone.Agent.Move;
using EventHorizon.Schedule;
using EventHorizon.Game.Server.Zone.Agent.State.Impl;
using EventHorizon.Game.Server.Zone.Agent.Move.Repository.Impl;
using EventHorizon.Game.Server.Zone.Agent.Move.Impl;
using EventHorizon.Game.Server.Zone.Agent.Save;
using EventHorizon.Game.Server.Zone.Tests.TestUtil;
using Microsoft.AspNetCore.Builder;
using System;
using MediatR;
using EventHorizon.Game.Server.Zone.Agent.Startup;
using System.Threading;
using EventHorizon.TimerService;
using EventHorizon.Game.Server.Zone.Agent.Model;
using EventHorizon.Game.Server.Zone.Agent.Connection;
using EventHorizon.Game.Server.Zone.Agent.Connection.Factory;

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
            var config = new ConfigurationSection(new ConfigurationRoot(new List<IConfigurationProvider>()), "Agent");
            configurationMock.Setup(a => a.GetSection("Agent")).Returns(config);
            new AgentSettings();

            // When
            AgentExtensions.AddAgent(serviceCollectionMock, configurationMock.Object);

            // Then
            Assert.NotEmpty(serviceCollectionMock);
            Assert.Contains(serviceCollectionMock.Services, a =>
            {
                return typeof(IAgentRepository) == a.Value.ServiceType
                    && typeof(AgentRepository) == a.Value.ImplementationType;
            });
            Assert.Contains(serviceCollectionMock.Services, a =>
            {
                return typeof(IMoveAgentRepository) == a.Value.ServiceType
                    && typeof(MoveAgentRepository) == a.Value.ImplementationType;
            });
            Assert.Contains(serviceCollectionMock.Services, a =>
            {
                return typeof(ITimerTask) == a.Value.ServiceType
                    && typeof(MoveRegisteredAgentsTimer) == a.Value.ImplementationType;
            });
            Assert.Contains(serviceCollectionMock.Services, a =>
            {
                return typeof(IScheduledTask) == a.Value.ServiceType
                    && typeof(SaveAgentStateScheduledTask) == a.Value.ImplementationType;
            });
            Assert.Contains(serviceCollectionMock.Services, a =>
            {
                return typeof(IAgentConnectionCache) == a.Value.ServiceType
                    && typeof(AgentConnectionCache) == a.Value.ImplementationType;
            });
            Assert.Contains(serviceCollectionMock.Services, a =>
            {
                return typeof(IAgentConnectionFactory) == a.Value.ServiceType
                    && typeof(AgentConnectionFactory) == a.Value.ImplementationType;
            });
        }
        [Fact]
        public void TestUseAgent_ShouldSendAndPublishExpectedEvent()
        {
            // Given
            var expectedLoadZoneAgentStateEvent = new LoadZoneAgentStateEvent();

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
            AgentExtensions.UseAgent(applicationBuilderMock.Object);

            // Then
            mediatorMock.Verify(mediator => mediator.Send(expectedLoadZoneAgentStateEvent, CancellationToken.None));
        }
    }
}