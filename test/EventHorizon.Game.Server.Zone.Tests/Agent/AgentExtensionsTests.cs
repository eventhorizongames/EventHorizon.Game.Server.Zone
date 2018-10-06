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

            // When
            AgentExtensions.AddAgent(serviceCollectionMock, configurationMock.Object);

            // Then
            Assert.NotEmpty(serviceCollectionMock);
            Assert.Collection(serviceCollectionMock,
                service =>
                {
                    Assert.Equal(typeof(IAgentRepository), service.ServiceType);
                    Assert.Equal(typeof(AgentRepository), service.ImplementationType);
                },
                service =>
                {
                    Assert.Equal(typeof(IMoveAgentRepository), service.ServiceType);
                    Assert.Equal(typeof(MoveAgentRepository), service.ImplementationType);
                },
                service =>
                {
                    Assert.Equal(typeof(ITimerTask), service.ServiceType);
                    Assert.Equal(typeof(MoveRegisteredAgentsTimer), service.ImplementationType);
                },
                service =>
                {
                    Assert.Equal(typeof(IScheduledTask), service.ServiceType);
                    Assert.Equal(typeof(SaveAgentStateScheduledTask), service.ImplementationType);
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