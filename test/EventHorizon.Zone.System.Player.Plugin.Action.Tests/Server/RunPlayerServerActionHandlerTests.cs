namespace EventHorizon.Zone.System.Player.Plugin.Action.Tests.Server
{
    using global::System.Collections.Generic;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using EventHorizon.Zone.Core.Model.Player;
    using EventHorizon.Zone.System.Player.Plugin.Action.Events;
    using EventHorizon.Zone.System.Player.Plugin.Action.Model;
    using EventHorizon.Zone.System.Player.Plugin.Action.Server;
    using EventHorizon.Zone.System.Player.Plugin.Action.State;

    using MediatR;

    using Moq;

    using Xunit;

    public class RunPlayerServerActionHandlerTests
    {
        [Fact]
        public async Task TestShouldPublishActionEventForEveryActionEventInPlayerRepositoryWhenEventIsHandled()
        {
            // Given
            var playerId = "player-id";
            var actionName = "action-name";
            var player = new PlayerEntity
            {
                PlayerId = playerId
            };
            var data = new Dictionary<string, object>();
            var action = new PlayerActionEntity(
                1,
                actionName,
                new TestPlayerActionEvent()
                    .SetPlayer(
                        player
                    ).SetData(
                        data
                    )
            );
            var actionList = new List<PlayerActionEntity>
            {
                action
            };
            var expected = new TestPlayerActionEvent()
                .SetData(
                    data
                ).SetPlayer(
                    player
                );

            var mediatorMock = new Mock<IMediator>();
            var playerRepositoryMock = new Mock<IPlayerRepository>();
            var playerActionRepositoryMock = new Mock<PlayerActionRepository>();

            playerRepositoryMock.Setup(
                mock => mock.FindById(
                    playerId
                )
            ).ReturnsAsync(
                player
            );
            playerActionRepositoryMock.Setup(
                mock => mock.Where(
                    actionName
                )
            ).Returns(
                actionList
            );

            // When
            var handler = new RunPlayerServerActionHandler(
                mediatorMock.Object,
                playerRepositoryMock.Object,
                playerActionRepositoryMock.Object
            );
            await handler.Handle(
                new RunPlayerServerAction(
                    playerId,
                    actionName,
                    data
                ),
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Publish(
                    expected,
                    CancellationToken.None
                )
            );
        }

        [Fact]
        public async Task TestShouldNotPublishActionEventWhenRepositoryReturnsEmptyActionList()
        {
            // Given
            var playerId = "player-id";
            var actionName = "action-name";
            var player = new PlayerEntity
            {
                PlayerId = playerId
            };
            var data = new Dictionary<string, object>();
            var actionList = new List<PlayerActionEntity>();

            var mediatorMock = new Mock<IMediator>();
            var playerRepositoryMock = new Mock<IPlayerRepository>();
            var playerActionRepositoryMock = new Mock<PlayerActionRepository>();

            playerRepositoryMock.Setup(
                mock => mock.FindById(
                    playerId
                )
            ).ReturnsAsync(
                player
            );
            playerActionRepositoryMock.Setup(
                mock => mock.Where(
                    actionName
                )
            ).Returns(
                actionList
            );

            // When
            var handler = new RunPlayerServerActionHandler(
                mediatorMock.Object,
                playerRepositoryMock.Object,
                playerActionRepositoryMock.Object
            );
            await handler.Handle(
                new RunPlayerServerAction(
                    playerId,
                    actionName,
                    data
                ),
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Publish(
                    It.IsAny<PlayerActionEvent>(),
                    CancellationToken.None
                ),
                Times.Never()
            );
        }

        [Fact]
        public async Task TestShouldNotPublishActionEventWhenPlayerIsNotFound()
        {
            // Given
            var playerId = "player-id";
            var actionName = "action-name";
            var player = default(PlayerEntity); // Default PlayerEntity will be flagged not found
            var data = new Dictionary<string, object>();
            var action = new PlayerActionEntity(
                1,
                actionName,
                new TestPlayerActionEvent()
                    .SetPlayer(
                        player
                    ).SetData(
                        data
                    )
            );
            var actionList = new List<PlayerActionEntity>
            {
                action
            };

            var mediatorMock = new Mock<IMediator>();
            var playerRepositoryMock = new Mock<IPlayerRepository>();
            var playerActionRepositoryMock = new Mock<PlayerActionRepository>();

            playerRepositoryMock.Setup(
                mock => mock.FindById(
                    playerId
                )
            ).ReturnsAsync(
                player
            );
            playerActionRepositoryMock.Setup(
                mock => mock.Where(
                    actionName
                )
            ).Returns(
                actionList
            );

            // When
            var handler = new RunPlayerServerActionHandler(
                mediatorMock.Object,
                playerRepositoryMock.Object,
                playerActionRepositoryMock.Object
            );
            await handler.Handle(
                new RunPlayerServerAction(
                    playerId,
                    actionName,
                    data
                ),
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Publish(
                    It.IsAny<PlayerActionEvent>(),
                    CancellationToken.None
                ),
                Times.Never()
            );
        }

        public struct TestPlayerActionEvent : PlayerActionEvent
        {
            public PlayerEntity Player { get; private set; }

            public IDictionary<string, object> Data { get; private set; }

            public PlayerActionEvent SetData(
                IDictionary<string, object> data
            )
            {
                Data = data;
                return this;
            }

            public PlayerActionEvent SetPlayer(
                PlayerEntity player
            )
            {
                this.Player = player;
                return this;
            }
        }
    }
}
