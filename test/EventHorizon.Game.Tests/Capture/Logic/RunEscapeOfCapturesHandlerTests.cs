namespace EventHorizon.Game.Tests.Capture.Logic
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using EventHorizon.Game.Capture.Logic;
    using EventHorizon.Game.Model;
    using EventHorizon.Zone.Core.Events.Entity.Update;
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.Core.Model.Player;
    using EventHorizon.Zone.System.Agent.Events.Get;
    using EventHorizon.Zone.System.Agent.Model;
    using EventHorizon.Zone.System.Agent.Plugin.Companion.Model;
    using EventHorizon.Zone.System.Combat.Plugin.Skill.Events.Runner;
    using MediatR;
    using Microsoft.AspNetCore.DataProtection.Cng.Internal;
    using Moq;
    using Xunit;

    public class RunEscapeOfCapturesHandlerTests
    {
        [Fact]
        public async Task ShouldSetOwnerIdToEmptyStringWhenProcessingRequestForPlayer()
        {
            // Given
            var playerId = "player-id";
            var agentEntity1GlobalId = "agent-entity-1";
            var agentEntity1 = new AgentEntity(
                new ConcurrentDictionary<string, object>()
            )
            {
                AgentId = agentEntity1GlobalId,
            }.SetProperty(
                OwnerState.PROPERTY_NAME,
                new OwnerState
                {
                    OwnerId = playerId,
                }
            );
            var agentEntity2GlobalId = "agent-entity-2";
            var agentEntity2 = new AgentEntity(
                new ConcurrentDictionary<string, object>()
            )
            {
                AgentId = agentEntity2GlobalId,
            }.SetProperty(
                OwnerState.PROPERTY_NAME,
                new OwnerState
                {
                    OwnerId = playerId,
                }
            );
            var agentEntity3GlobalId = "agent-entity-3";
            var agentEntity3 = new AgentEntity(
                new ConcurrentDictionary<string, object>()
            )
            {
                AgentId = agentEntity3GlobalId,
            }.SetProperty(
                OwnerState.PROPERTY_NAME,
                new OwnerState
                {
                    OwnerId = playerId,
                }
            );
            var playerEntity = new PlayerEntity();
            playerEntity = playerEntity.SetProperty(
                GamePlayerCaptureState.PROPERTY_NAME,
                new GamePlayerCaptureState
                {
                    CompanionsCaught = new List<string>
                    {
                        agentEntity1GlobalId,
                        agentEntity2GlobalId,
                        agentEntity3GlobalId,
                    }
                }
            );
            var expected = "";

            var mediatorMock = new Mock<IMediator>();

            mediatorMock.Setup(
                mock => mock.Send(
                    new FindAgentByIdEvent(
                        agentEntity1GlobalId
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                agentEntity1
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    new FindAgentByIdEvent(
                        agentEntity2GlobalId
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                agentEntity2
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    new FindAgentByIdEvent(
                        agentEntity3GlobalId
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                agentEntity3
            );

            // When
            var handler = new RunEscapeOfCapturesHandler(
                mediatorMock.Object
            );
            await handler.Handle(
                new RunEscapeOfCaptures(
                    playerEntity
                ),
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Send(
                    It.Is<UpdateEntityCommand>(
                        evnt => evnt.Action == EntityAction.PROPERTY_CHANGED
                            && VerifyEntityOwnerIsExpected(
                                evnt.Entity,
                                expected
                            )
                    ),
                    CancellationToken.None
                ),
                Times.Exactly(3)
            );
        }

        [Fact]
        public async Task ShouldFireComanionsEscapedSkillWhenRequestIsHandled()
        {
            // Given
            var connectionId = "player-connection-id";
            var skillId = "skill_id";
            var playerEntityId = 123L;
            var playerEntity = new PlayerEntity
            {
                Id = playerEntityId,
                ConnectionId = connectionId
            };
            playerEntity = playerEntity.SetProperty(
                GamePlayerCaptureState.PROPERTY_NAME,
                new GamePlayerCaptureState
                {
                    CompanionsCaught = new List<string>(),
                }
            );

            var mediatorMock = new Mock<IMediator>();


            // When
            var handler = new RunEscapeOfCapturesHandler(
                mediatorMock.Object
            );
            await handler.Handle(
                new RunEscapeOfCaptures(
                    playerEntity
                ),
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Publish(
                    new RunSkillWithTargetOfEntityEvent
                    {
                        ConnectionId = connectionId,
                        SkillId = skillId,
                        CasterId = playerEntityId,
                        TargetId = playerEntityId,
                    },
                    CancellationToken.None
                )
            );
        }

        private bool VerifyEntityOwnerIsExpected(
            IObjectEntity entity,
            string expected
        )
        {
            var ownerState = entity.GetProperty<OwnerState>(
                OwnerState.PROPERTY_NAME
            );
            return expected.Equals(
                ownerState.OwnerId
            );
        }
    }
}
