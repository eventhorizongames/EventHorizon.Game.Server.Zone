using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.Entity.Find;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.Core.Model.Player;
using EventHorizon.Zone.System.Combat.Events.Client.Messsage;
using EventHorizon.Zone.System.Combat.Model.Client.Messsage;
using EventHorizon.Zone.System.Interaction.Events;
using EventHorizon.Zone.System.Interaction.Model;
using EventHorizon.Zone.System.Interaction.Run;
using EventHorizon.Zone.System.Interaction.Script.Run;
using MediatR;
using Moq;
using Xunit;

namespace EventHorizon.Zone.System.Interaction.Tests.Run
{
    public class RunInteractionCommandHandlerTests
    {
        [Fact]
        public async Task TestShouldPublishInvalidInteractionWhenNotInteractionEntityIsNotFound()
        {
            // Given
            var expectedEvent = new SingleClientActionMessageFromCombatSystemEvent
            {
                ConnectionId = "connection-id",
                Data = new MessageFromCombatSystemData
                {
                    MessageCode = "interaction_not_valid",
                    Message = "Interaction entity was not found."
                }
            };
            var interactionEntityId = 123L;
            var playerEntity = new PlayerEntity
            {
                ConnectionId = "connection-id"
            };

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(
                mock => mock.Send(
                    new GetEntityByIdEvent
                    {
                        EntityId = interactionEntityId
                    },
                    CancellationToken.None
                )
            ).ReturnsAsync(
                default(DefaultEntity)
            );

            // When
            var handler = new RunInteractionCommandHandler(
                mediatorMock.Object
            );
            await handler.Handle(
                new RunInteractionCommand(
                    interactionEntityId,
                    playerEntity
                ),
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Publish(
                    expectedEvent,
                    CancellationToken.None
                )
            );
        }
        [Fact]
        public async Task TestShouldIgnoreRequestWhenPlayerEntityIsNotFound()
        {
            // Given
            var interactionEntityId = 123L;
            var playerEntity = default(PlayerEntity);

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(
                mock => mock.Send(
                    new GetEntityByIdEvent
                    {
                        EntityId = interactionEntityId
                    },
                    CancellationToken.None
                )
            ).ReturnsAsync(
                new DefaultEntity()
            );

            // When
            var handler = new RunInteractionCommandHandler(
                mediatorMock.Object
            );
            await handler.Handle(
                new RunInteractionCommand(
                    interactionEntityId,
                    playerEntity
                ),
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Publish(
                    It.IsAny<INotification>(),
                    CancellationToken.None
                ),
                Times.Never()
            );
        }
        [Fact]
        public async Task TestShouldPublishInvalidInteractionWhenInteractionIsNotActive()
        {
            // Given
            var expectedEvent = new SingleClientActionMessageFromCombatSystemEvent
            {
                ConnectionId = "connection-id",
                Data = new MessageFromCombatSystemData
                {
                    MessageCode = "interaction_not_active",
                    Message = "Interaction was not active."
                }
            };
            var interactionEntityId = 123L;
            var playerEntity = new PlayerEntity
            {
                ConnectionId = "connection-id"
            };
            var interactionEntity = new DefaultEntity(
                new Dictionary<string, object>()
            )
            {
                Id = interactionEntityId
            };
            interactionEntity.PopulateData(
                "interactionState",
                new InteractionState
                {
                    Active = false
                }
            );

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(
                mock => mock.Send(
                    new GetEntityByIdEvent
                    {
                        EntityId = interactionEntityId
                    },
                    CancellationToken.None
                )
            ).ReturnsAsync(
                interactionEntity
            );

            // When
            var handler = new RunInteractionCommandHandler(
                mediatorMock.Object
            );
            await handler.Handle(
                new RunInteractionCommand(
                    interactionEntityId,
                    playerEntity
                ),
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Publish(
                    expectedEvent,
                    CancellationToken.None
                )
            );
        }
        [Fact]
        public async Task TestShouldRunInteractionScriptCommandsWhithListOfInteractionItems()
        {
            // Given
            var expectedScriptId = "script-id";
            var expectedDistanceToPlayer = 1;
            var expectedData = new Dictionary<string, object>();
            var expectedInteractionItem = new InteractionItem
            {
                ScriptId = expectedScriptId,
                DistanceToPlayer = expectedDistanceToPlayer,
                Data = expectedData
            };
            var interactionEntityId = 123L;
            var playerEntity = new PlayerEntity
            {
                ConnectionId = "connection-id"
            };
            var interactionEntity = new DefaultEntity(
                new Dictionary<string, object>()
            )
            {
                Id = interactionEntityId
            };
            interactionEntity.PopulateData(
                "interactionState",
                new InteractionState
                {
                    Active = true,
                    List = new List<InteractionItem>
                    {
                        expectedInteractionItem
                    }
                }
            );

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(
                mock => mock.Send(
                    new GetEntityByIdEvent
                    {
                        EntityId = interactionEntityId
                    },
                    CancellationToken.None
                )
            ).ReturnsAsync(
                interactionEntity
            );

            // When
            var handler = new RunInteractionCommandHandler(
                mediatorMock.Object
            );
            await handler.Handle(
                new RunInteractionCommand(
                    interactionEntityId,
                    playerEntity
                ),
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Send(
                    new RunInteractionScriptCommand(
                        expectedInteractionItem,
                        interactionEntity,
                        playerEntity
                    ),
                    CancellationToken.None
                )
            );
        }
    }
}