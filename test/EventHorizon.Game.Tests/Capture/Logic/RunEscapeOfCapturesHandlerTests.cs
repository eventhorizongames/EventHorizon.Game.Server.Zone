namespace EventHorizon.Game.Tests.Capture.Logic
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Numerics;
    using System.Threading;
    using System.Threading.Tasks;

    using EventHorizon.Game.Capture.Logic;
    using EventHorizon.Game.Clear;
    using EventHorizon.Game.Model;
    using EventHorizon.Zone.Core.Events.Entity.Update;
    using EventHorizon.Zone.Core.Events.Map.Cost;
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.Core.Model.Player;
    using EventHorizon.Zone.System.Agent.Events.Get;
    using EventHorizon.Zone.System.Agent.Model;
    using EventHorizon.Zone.System.Agent.Plugin.Companion.Model;
    using EventHorizon.Zone.System.Combat.Plugin.Skill.Events.Runner;

    using FluentAssertions;

    using MediatR;

    using Moq;

    using Xunit;
    using Xunit.Sdk;

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
            var skillId = SkillConstants.ESCAPE_OF_CAPTURES_SKILL_ID;
            var playerEntityId = 123L;
            var position = Vector3.One;
            var playerEntity = new PlayerEntity
            {
                Id = playerEntityId,
                ConnectionId = connectionId,
                Transform = new Zone.Core.Model.Core.TransformState
                {
                    Position = position,
                }
            };
            playerEntity = playerEntity.SetProperty(
                GamePlayerCaptureState.PROPERTY_NAME,
                new GamePlayerCaptureState
                {
                    CompanionsCaught = new List<string>(),
                }
            );
            var expectedConnectionId = connectionId;
            var expectedSkillId = skillId;
            var expectedCasterId = playerEntityId;
            var expectedTargetId = playerEntityId;
            var expectedTargetPosition = position;
            var expectedDataKey = "game:MessageKey";
            var expectedDataValue = "game:CapturesEscaped";

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
                    It.Is<RunSkillWithTargetOfEntityEvent>(
                        a => a.ConnectionId == expectedConnectionId
                            && a.SkillId == expectedSkillId
                            && a.CasterId == expectedCasterId
                            && a.TargetId == expectedTargetId
                            && a.TargetPosition == expectedTargetPosition
                            && a.Data.Count == 1
                            && (string)a.Data[expectedDataKey] == expectedDataValue
                    ),
                    CancellationToken.None
                )
            );
        }

        [Fact]
        public async Task ShouldSetPlayerGameCaptureStateToNewWhenRequestIsHandled()
        {
            // Given
            var connectionId = "player-connection-id";
            var skillId = SkillConstants.ESCAPE_OF_CAPTURES_SKILL_ID;
            var playerEntityId = 123L;
            var position = Vector3.One;
            var playerEntity = new PlayerEntity
            {
                Id = playerEntityId,
                ConnectionId = connectionId,
                Transform = new Zone.Core.Model.Core.TransformState
                {
                    Position = position,
                }
            };
            playerEntity = playerEntity.SetProperty(
                GamePlayerCaptureState.PROPERTY_NAME,
                new GamePlayerCaptureState
                {
                    CompanionsCaught = new List<string>(),
                }
            );
            var expected = GamePlayerCaptureState.New();

            var mediatorMock = new Mock<IMediator>();

            // When
            var handler = new RunEscapeOfCapturesHandler(
                mediatorMock.Object
            );
            var request = default(UpdateEntityCommand);
            mediatorMock.Setup(
                mock => mock.Send(
                    It.IsAny<UpdateEntityCommand>(),
                    CancellationToken.None
                )
            ).Callback<IRequest<Unit>, CancellationToken>(
                (requestUnit, _) =>
                {
                    request = (UpdateEntityCommand)requestUnit;
                }
            );
            await handler.Handle(
                new RunEscapeOfCaptures(
                    playerEntity
                ),
                CancellationToken.None
            );
            var actual = request.Entity.GetProperty<GamePlayerCaptureState>(
                GamePlayerCaptureState.PROPERTY_NAME
            );

            // Then
            actual.Captures
                .Should().Be(expected.Captures);
            actual.CompanionsCaught
                .Should().BeEquivalentTo(expected.CompanionsCaught);
            actual.EscapeCaptureTime
                .Should().Be(expected.EscapeCaptureTime);
            actual.ShownFiveSecondMessage
                .Should().Be(expected.ShownFiveSecondMessage);
            actual.ShownTenSecondMessage
                .Should().Be(expected.ShownTenSecondMessage);
        }

        [Fact]
        public async Task ShouldClearPlayerScoreWhenRequestIsHandled()
        {
            // Given
            var connectionId = "player-connection-id";
            var skillId = SkillConstants.ESCAPE_OF_CAPTURES_SKILL_ID;
            var playerEntityId = 123L;
            var position = Vector3.One;
            var playerEntity = new PlayerEntity
            {
                Id = playerEntityId,
                ConnectionId = connectionId,
                Transform = new Zone.Core.Model.Core.TransformState
                {
                    Position = position,
                }
            };
            playerEntity = playerEntity.SetProperty(
                GamePlayerCaptureState.PROPERTY_NAME,
                new GamePlayerCaptureState
                {
                    CompanionsCaught = new List<string>(),
                }
            );
            var expected = new ClearPlayerScore(
                playerEntityId
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
                mock => mock.Send(
                    expected,
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
