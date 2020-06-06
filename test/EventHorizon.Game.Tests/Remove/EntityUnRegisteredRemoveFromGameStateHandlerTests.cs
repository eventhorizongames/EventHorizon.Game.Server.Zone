namespace EventHorizon.Game.Tests.Remove
{
    using System.Threading;
    using System.Threading.Tasks;
    using EventHorizon.Game.Clear;
    using EventHorizon.Game.Remove;
    using EventHorizon.Game.State;
    using EventHorizon.Zone.Core.Events.Entity.Register;
    using MediatR;
    using Moq;
    using Xunit;

    public class EntityUnRegisteredRemoveFromGameStateHandlerTests
    {
        [Fact]
        public async Task ShouldSendClearPlayerScoreWhenNotificationIsHandled()
        {
            // Given
            var entityId = 123L;
            var expected = new ClearPlayerScore(
                entityId
            );

            var mediatorMock = new Mock<IMediator>();
            var gameStateMock = new Mock<GameState>();

            // When
            var handler = new EntityUnRegisteredRemoveFromGameStateHandler(
                mediatorMock.Object
            );
            await handler.Handle(
                new EntityUnRegisteredEvent(
                    entityId
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
    }
}
