namespace EventHorizon.Game.Tests.Capture.Logic
{
    using System.Threading;
    using System.Threading.Tasks;

    using EventHorizon.Game.Capture.Logic;
    using EventHorizon.Game.Model;
    using EventHorizon.Game.Model.Client;
    using EventHorizon.Zone.Core.Events.Client.Generic;
    using EventHorizon.Zone.Core.Events.Entity.Update;
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.Core.Model.Player;

    using FluentAssertions;

    using MediatR;

    using Moq;

    using Xunit;

    public class ProcessFiveSecondCaptureLogicHandlerTests
    {
        [Fact]
        public async Task ShouldPublishClientActionWheRequestIsHandled()
        {
            // Given
            var connectionId = "player-connection-id";
            var playerEntity = new PlayerEntity
            {
                ConnectionId = connectionId
            };
            var expected = new ClientActionGenericToSingleEvent(
                connectionId,
                "Server.SHOW_FIVE_SECOND_CAPTURE_MESSAGE",
                new ClientActionShowFiveSecondCaptureMessageData()
            );

            var mediatorMock = new Mock<IMediator>();

            // When
            var handler = new ProcessFiveSecondCaptureLogicHandler(
                mediatorMock.Object
            );
            await handler.Handle(
                new ProcessFiveSecondCaptureLogic(
                    playerEntity
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
        public async Task ShouldPublishUpdateEntityGamePlayerCaptureStatePropertyChangeWhenSuccessful()
        {
            // Given
            var connectionId = "player-connection-id";
            var playerEntity = new PlayerEntity
            {
                ConnectionId = connectionId
            };
            var captureState = new GamePlayerCaptureState
            {
                ShownFiveSecondMessage = false
            };
            playerEntity.SetProperty(
                GamePlayerCaptureState.PROPERTY_NAME,
                captureState
            );
            var expected = new GamePlayerCaptureState()
            {
                ShownFiveSecondMessage = true,
            };

            var mediatorMock = new Mock<IMediator>();

            // When
            var handler = new ProcessFiveSecondCaptureLogicHandler(
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
                new ProcessFiveSecondCaptureLogic(
                    playerEntity
                ),
                CancellationToken.None
            );
            var acutal = request.Entity.GetProperty<GamePlayerCaptureState>(
                GamePlayerCaptureState.PROPERTY_NAME
            );

            // Then
            acutal.Should().Be(expected);
        }
    }
}
