using Xunit;
using Moq;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using EventHorizon.Performance;
using EventHorizon.Zone.System.Agent.Model.State;
using EventHorizon.Zone.System.Agent.Move.Register;
using EventHorizon.Zone.System.Agent.Plugin.Move.Events;

namespace EventHorizon.Game.Server.Zone.Tests.Agent.Move.Handler
{
    public class MoveRegisteredAgentsHandlerTests
    {
        private delegate void IdSetter(out long entityId);


        [Fact]
        public async Task TestHandle_ShouldPublishToMoveRegisteredAgentForEachAgentInRepository()
        {
            // Given
            var inputId1 = 111L;
            var inputId2 = 222L;
            var inputId3 = 333L;
            var expectedMoveRegisteredAgentEvent1 = new MoveRegisteredAgentEvent
            {
                EntityId = inputId1
            };
            var expectedMoveRegisteredAgentEvent2 = new MoveRegisteredAgentEvent
            {
                EntityId = inputId2
            };
            var expectedMoveRegisteredAgentEvent3 = new MoveRegisteredAgentEvent
            {
                EntityId = inputId3
            };
            var entityIdList = new List<long>()
            {
                inputId1,
                inputId2,
                inputId3
            };

            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<MoveRegisteredAgentsHandler>>();
            var moveRepositoryMock = new Mock<IMoveAgentRepository>();
            var called = 0;
            moveRepositoryMock.Setup(
                moveRepository => moveRepository.Dequeue(
                    out inputId1
                )
            ).Callback(
                new IdSetter((out long id) =>
                {
                    if (called == 0)
                        id = inputId1;
                    else if (called == 1)
                        id = inputId2;
                    else if (called == 2)
                        id = inputId3;
                    else
                        id = -1;
                    called++;
                })
            ).Returns(
                () => called <= 3
            );
            // When
            var moveRegisteredAgentsHandler = new MoveRegisteredAgentsHandler(
                loggerMock.Object,
                mediatorMock.Object,
                moveRepositoryMock.Object,
                new Mock<IPerformanceTracker>().Object
            );
            await moveRegisteredAgentsHandler.Handle(
                new MoveRegisteredAgentsEvent(),
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mediator => mediator.Publish(
                    It.IsAny<MoveRegisteredAgentEvent>(),
                    It.IsAny<CancellationToken>()
                ),
                Times.Exactly(
                    3
                )
            );
            mediatorMock.Verify(
                mediator => mediator.Publish(
                    expectedMoveRegisteredAgentEvent1,
                    CancellationToken.None
                )
            );
            mediatorMock.Verify(
                mediator => mediator.Publish(
                    expectedMoveRegisteredAgentEvent2,
                    CancellationToken.None
                )
            );
            mediatorMock.Verify(
                mediator => mediator.Publish(
                    expectedMoveRegisteredAgentEvent3,
                    CancellationToken.None
                )
            );
        }
        
        [Fact]
        public async Task TestHandle_ShouldNotPublishToMoveRegisteredAgentWhenNothingIsInAgentRepository()
        {
            // Given
            var defaultEntityId = 0L;
            var entityIdList = new List<long>()
            {
            };

            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<MoveRegisteredAgentsHandler>>();
            var moveRepositoryMock = new Mock<IMoveAgentRepository>();

            moveRepositoryMock.Setup(
                moveRepository => moveRepository.Dequeue(
                    out defaultEntityId
                )
            ).Returns(
                false
            );

            // When
            var moveRegisteredAgentsHandler = new MoveRegisteredAgentsHandler(
                loggerMock.Object,
                mediatorMock.Object,
                moveRepositoryMock.Object,
                new Mock<IPerformanceTracker>().Object
            );
            await moveRegisteredAgentsHandler.Handle(
                new MoveRegisteredAgentsEvent(),
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mediator => mediator.Publish(
                    It.IsAny<MoveRegisteredAgentEvent>(),
                    CancellationToken.None
                ),
                Times.Never()
            );
        }

        [Fact]
        public async Task TestShouldNotBubbleExceptionsWhenMoveRegisteredAgentEventThrownsAnyException()
        {
            // Given
            var entityIdList = new List<long>();

            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<MoveRegisteredAgentsHandler>>();
            var moveRepositoryMock = new Mock<IMoveAgentRepository>();

            for (long i = 0; i < 26; i++)
            {
                entityIdList.Add(i);
                moveRepositoryMock.Setup(
                    moveRepository => moveRepository.Dequeue(
                        out i
                    )
                ).Returns(true);
            }

            mediatorMock.Setup(
                mock => mock.Publish(
                    It.IsAny<MoveRegisteredAgentEvent>(),
                    CancellationToken.None
                )
            ).ThrowsAsync(
                new Exception(
                    "error"
                )
            );

            // When
            var moveRegisteredAgentsHandler = new MoveRegisteredAgentsHandler(
                loggerMock.Object,
                mediatorMock.Object,
                moveRepositoryMock.Object,
                new Mock<IPerformanceTracker>().Object
            );
            await moveRegisteredAgentsHandler.Handle(
                new MoveRegisteredAgentsEvent(),
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mediator => mediator.Publish(
                    It.IsAny<MoveRegisteredAgentEvent>(),
                    It.IsAny<CancellationToken>()
                ),
                Times.AtLeast(
                    25
                )
            );
        }

        [Fact]
        public async Task TestShouldNotThrowExceptionWhenAgentCountListIsOver25Agents()
        {
            // Given
            var entityIdList = new List<long>();

            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<MoveRegisteredAgentsHandler>>();
            var moveRepositoryMock = new Mock<IMoveAgentRepository>();

            for (long i = 0; i < 76; i++)
            {
                entityIdList.Add(i);
                moveRepositoryMock.Setup(
                    moveRepository => moveRepository.Dequeue(
                        out i
                    )
                ).Returns(true);
            }

            // When
            var moveRegisteredAgentsHandler = new MoveRegisteredAgentsHandler(
                loggerMock.Object,
                mediatorMock.Object,
                moveRepositoryMock.Object,
                new Mock<IPerformanceTracker>().Object
            );
            await moveRegisteredAgentsHandler.Handle(
                new MoveRegisteredAgentsEvent(),
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mediator => mediator.Publish(
                    It.IsAny<MoveRegisteredAgentEvent>(),
                    It.IsAny<CancellationToken>()
                ),
                Times.AtLeast(
                    25
                )
            );
        }
    }
}