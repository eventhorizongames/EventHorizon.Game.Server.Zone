namespace EventHorizon.Game.Tests.Capture.Logic
{
    using System.Threading;
    using System.Threading.Tasks;
    using EventHorizon.Game.Capture.Logic;
    using EventHorizon.Game.Model.Client;
    using EventHorizon.Zone.Core.Events.Client.Generic;
    using EventHorizon.Zone.Core.Model.Player;
    using MediatR;
    using Moq;
    using Xunit;

    public class ProcessTenSecondCaptureLogicHandlerTests
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
                "Server.SHOW_TEN_SECOND_CAPTURE_MESSAGE",
                new ClientActionShowTenSecondCaptureMessageData()
            );

            var mediatorMock = new Mock<IMediator>();

            // When
            var handler = new ProcessTenSecondCaptureLogicHandler(
                mediatorMock.Object
            );
            await handler.Handle(
                new ProcessTenSecondCaptureLogic(
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
    }
}
