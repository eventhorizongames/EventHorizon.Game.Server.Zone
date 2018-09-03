using Xunit;
using Moq;
using MediatR;
using EventHorizon.Game.Server.Zone.Agent.Move;
using System.Threading.Tasks;
using System.Threading;
using EventHorizon.Game.Server.Zone.Agent.Move.Handler;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using EventHorizon.Game.Server.Zone.Agent.Move.Repository;
using System.Collections.Generic;

namespace EventHorizon.Game.Server.Zone.Tests.Agent.Move.Handler
{
    public class MoveRegisteredAgentsHandlerTests
    {
        [Fact]
        public async Task TestHandle_ShouldPublishToMoveRegisteredAgentForEachAgentInRepository()
        {
            // Given
            var inputId1 = 111;
            var inputId2 = 222;
            var inputId3 = 333;
            var expectedMoveRegisteredAgentEvent1 = new MoveRegisteredAgentEvent
            {
                AgentId = inputId1
            };
            var expectedMoveRegisteredAgentEvent2 = new MoveRegisteredAgentEvent
            {
                AgentId = inputId2
            };
            var expectedMoveRegisteredAgentEvent3 = new MoveRegisteredAgentEvent
            {
                AgentId = inputId3
            };
            var entityIdList = new List<long>()
            {
                inputId1,
                inputId2,
                inputId3
            };

            var mediatorMock = new Mock<IMediator>();
            var serviceScopeMock = new Mock<IServiceScope>();
            var serviceProviderMock = new Mock<IServiceProvider>();

            serviceProviderMock.Setup(serviceProvider => serviceProvider.GetService(typeof(IMediator))).Returns(mediatorMock.Object);
            serviceScopeMock.SetupGet(serviceScope => serviceScope.ServiceProvider).Returns(serviceProviderMock.Object);

            var loggerMock = new Mock<ILogger<MoveRegisteredAgentsHandler>>();
            var moveRepositoryMock = new Mock<IMoveAgentRepository>();
            var serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();

            moveRepositoryMock.Setup(moveRepository => moveRepository.All()).Returns(entityIdList);
            serviceScopeFactoryMock.Setup(serviceScopeFactory => serviceScopeFactory.CreateScope()).Returns(serviceScopeMock.Object);

            // When
            var moveRegisteredAgentsHandler = new MoveRegisteredAgentsHandler(
                loggerMock.Object,
                moveRepositoryMock.Object,
                serviceScopeFactoryMock.Object
            );
            await moveRegisteredAgentsHandler.Handle(new MoveRegisteredAgentsEvent(), CancellationToken.None);

            // Then
            mediatorMock.Verify(mediator => mediator.Publish(expectedMoveRegisteredAgentEvent1, CancellationToken.None));
            mediatorMock.Verify(mediator => mediator.Publish(expectedMoveRegisteredAgentEvent2, CancellationToken.None));
            mediatorMock.Verify(mediator => mediator.Publish(expectedMoveRegisteredAgentEvent3, CancellationToken.None));
        }
        [Fact]
        public async Task TestHandle_ShouldNotPublishToMoveRegisteredAgentWhenNothingIsInAgentRepository()
        {
            // Given
            var entityIdList = new List<long>()
            {
            };

            var mediatorMock = new Mock<IMediator>();
            var serviceScopeMock = new Mock<IServiceScope>();
            var serviceProviderMock = new Mock<IServiceProvider>();

            serviceProviderMock.Setup(serviceProvider => serviceProvider.GetService(typeof(IMediator))).Returns(mediatorMock.Object);
            serviceScopeMock.SetupGet(serviceScope => serviceScope.ServiceProvider).Returns(serviceProviderMock.Object);

            var loggerMock = new Mock<ILogger<MoveRegisteredAgentsHandler>>();
            var moveRepositoryMock = new Mock<IMoveAgentRepository>();
            var serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();

            moveRepositoryMock.Setup(moveRepository => moveRepository.All()).Returns(entityIdList);
            serviceScopeFactoryMock.Setup(serviceScopeFactory => serviceScopeFactory.CreateScope()).Returns(serviceScopeMock.Object);

            // When
            var moveRegisteredAgentsHandler = new MoveRegisteredAgentsHandler(
                loggerMock.Object,
                moveRepositoryMock.Object,
                serviceScopeFactoryMock.Object
            );
            await moveRegisteredAgentsHandler.Handle(new MoveRegisteredAgentsEvent(), CancellationToken.None);

            // Then
            mediatorMock.Verify(mediator => mediator.Publish(It.IsAny<MoveRegisteredAgentEvent>(), CancellationToken.None), Times.Never());
        }
    }
}