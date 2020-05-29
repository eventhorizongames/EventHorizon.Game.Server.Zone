namespace EventHorizon.Game.Tests.Capture
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using EventHorizon.Game.Capture;
    using EventHorizon.Zone.Core.Model.Player;
    using EventHorizon.Zone.Core.Model.Entity;
    using Xunit;
    using EventHorizon.Game.Model;
    using Moq;
    using MediatR;
    using EventHorizon.Zone.System.Player.Events.Find;
    using System;
    using EventHorizon.Game.Capture.Logic;
    using EventHorizon.Zone.Core.Model.DateTimeService;

    // ClientActionShowTenSecondCaptureMessage
    /// | Capture Resets to 30s
    /// |                                now                |
    /// | Captured (+9s)               9s left              | 9s from now
    /// |
    /// | Captured (+9s) <> now.AddSeconds(10) >= 0
    public class RunCaptureLogicForPlayerHandlerTests
    {
        [Fact]
        public async Task ShouldReturnWithoutRunningLogicWhenCapturesAreZeroWhenRequestIsHandled()
        {
            // Given
            var playerEntityId = 123L;
            var captures = 0;
            var playerEntity = new PlayerEntity
            {
                Id = playerEntityId,
                RawData = new ConcurrentDictionary<string, object>(),
            };
            playerEntity.SetProperty(
                GamePlayerCaptureState.PROPERTY_NAME,
                new GamePlayerCaptureState
                {
                    Captures = captures,
                }
            );

            var mediatorMock = new Mock<IMediator>();
            var dateTimeMock = new Mock<IDateTimeService>();

            mediatorMock.Setup(
                mock => mock.Send(
                    new FindPlayerByEntityId(
                        playerEntityId
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                playerEntity
            );

            // When
            var handler = new RunCaptureLogicForPlayerHandler(
                mediatorMock.Object,
                dateTimeMock.Object
            );
            await handler.Handle(
                new RunCaptureLogicForPlayer(
                    playerEntityId
                ),
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Send(
                    new FindPlayerByEntityId(
                        playerEntityId
                    ),
                    CancellationToken.None
                )
            );
            mediatorMock.Verify(
                mock => mock.Send(
                    new ProcessTenSecondCaptureLogic(
                        playerEntity
                    ),
                    CancellationToken.None
                ),
                Times.Never()
            );
            mediatorMock.Verify(
                mock => mock.Send(
                    new ProcessFiveSecondCaptureLogic(
                        playerEntity
                    ),
                    CancellationToken.None
                ),
                Times.Never()
            );
        }

        [Fact]
        public async Task ShouldSendProcessTenSecondCaptureLogicWhenLessThenTenSecondsSinceLastCaptureAndHasNotShownTenSecondMessage()
        {
            // Given
            var playerEntityId = 123L;
            var captures = 1;
            var showTenSecondCaptureMessage = false;
            var showFiveSecondCaptureMessage = false;
            var now = DateTime.Now;

            var escapeCaptureTime = now.AddSeconds(9);

            var playerEntity = new PlayerEntity
            {
                Id = playerEntityId,
                RawData = new ConcurrentDictionary<string, object>(),
            };
            playerEntity.SetProperty(
                GamePlayerCaptureState.PROPERTY_NAME,
                new GamePlayerCaptureState
                {
                    EscapeCaptureTime = escapeCaptureTime,
                    ShownTenSecondMessage = showTenSecondCaptureMessage,
                    ShownFiveSecondMessage = showFiveSecondCaptureMessage,
                    Captures = captures,
                }
            );

            var mediatorMock = new Mock<IMediator>();
            var dateTimeMock = new Mock<IDateTimeService>();

            mediatorMock.Setup(
                mock => mock.Send(
                    new FindPlayerByEntityId(
                        playerEntityId
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                playerEntity
            );

            dateTimeMock.Setup(
                mock => mock.Now
            ).Returns(
                now
            );

            // When
            var handler = new RunCaptureLogicForPlayerHandler(
                mediatorMock.Object,
                dateTimeMock.Object
            );
            await handler.Handle(
                new RunCaptureLogicForPlayer(
                    playerEntityId
                ),
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Send(
                    new ProcessTenSecondCaptureLogic(
                        playerEntity
                    ),
                    CancellationToken.None
                )
            );
        }

        [Fact]
        public async Task ShouldSendProcessFiveSecondCaptureLogicWhenLessThenFiveSecondsSinceLastCaptureAndHasNotShownFiveSecondMessage()
        {
            // Given
            var playerEntityId = 123L;
            var captures = 1;
            var showTenSecondCaptureMessage = true;
            var showFiveSecondCaptureMessage = false;
            var now = DateTime.Now;

            var escapeCaptureTime = now.AddSeconds(4);

            var playerEntity = new PlayerEntity
            {
                Id = playerEntityId,
                RawData = new ConcurrentDictionary<string, object>(),
            };
            playerEntity.SetProperty(
                GamePlayerCaptureState.PROPERTY_NAME,
                new GamePlayerCaptureState
                {
                    EscapeCaptureTime = escapeCaptureTime,
                    ShownTenSecondMessage = showTenSecondCaptureMessage,
                    ShownFiveSecondMessage = showFiveSecondCaptureMessage,
                    Captures = captures,
                }
            );

            var mediatorMock = new Mock<IMediator>();
            var dateTimeMock = new Mock<IDateTimeService>();

            mediatorMock.Setup(
                mock => mock.Send(
                    new FindPlayerByEntityId(
                        playerEntityId
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                playerEntity
            );

            dateTimeMock.Setup(
                mock => mock.Now
            ).Returns(
                now
            );

            // When
            var handler = new RunCaptureLogicForPlayerHandler(
                mediatorMock.Object,
                dateTimeMock.Object
            );
            await handler.Handle(
                new RunCaptureLogicForPlayer(
                    playerEntityId
                ),
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Send(
                    new ProcessFiveSecondCaptureLogic(
                        playerEntity
                    ),
                    CancellationToken.None
                )
            );
            mediatorMock.Verify(
                mock => mock.Send(
                    new ProcessTenSecondCaptureLogic(
                        playerEntity
                    ),
                    CancellationToken.None
                ),
                Times.Never()
            );
        }

        [Fact]
        public async Task ShouldDoNothingWhenHaveCapturesAndNotInAnyTimeTriggers()
        {
            // Given
            var playerEntityId = 123L;
            var captures = 1;
            var showTenSecondCaptureMessage = false;
            var showFiveSecondCaptureMessage = false;
            var now = DateTime.Now;

            var escapeCaptureTime = now.AddSeconds(30);

            var playerEntity = new PlayerEntity
            {
                Id = playerEntityId,
                RawData = new ConcurrentDictionary<string, object>(),
            };
            playerEntity.SetProperty(
                GamePlayerCaptureState.PROPERTY_NAME,
                new GamePlayerCaptureState
                {
                    EscapeCaptureTime = escapeCaptureTime,
                    ShownTenSecondMessage = showTenSecondCaptureMessage,
                    ShownFiveSecondMessage = showFiveSecondCaptureMessage,
                    Captures = captures,
                }
            );

            var mediatorMock = new Mock<IMediator>();
            var dateTimeMock = new Mock<IDateTimeService>();

            mediatorMock.Setup(
                mock => mock.Send(
                    new FindPlayerByEntityId(
                        playerEntityId
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                playerEntity
            );

            dateTimeMock.Setup(
                mock => mock.Now
            ).Returns(
                now
            );

            // When
            var handler = new RunCaptureLogicForPlayerHandler(
                mediatorMock.Object,
                dateTimeMock.Object
            );
            await handler.Handle(
                new RunCaptureLogicForPlayer(
                    playerEntityId
                ),
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Send(
                    new ProcessFiveSecondCaptureLogic(
                        playerEntity
                    ),
                    CancellationToken.None
                ),
                Times.Never()
            );
            mediatorMock.Verify(
                mock => mock.Send(
                    new ProcessTenSecondCaptureLogic(
                        playerEntity
                    ),
                    CancellationToken.None
                ),
                Times.Never()
            );
            mediatorMock.Verify(
                mock => mock.Send(
                    new RunEscapeOfCaptures(
                        playerEntity
                    ),
                    CancellationToken.None
                ),
                Times.Never()
            );
        }

        [Fact]
        public async Task ShouldSendProcessFiveAndTenSecondCaptureLogicWhenLessThenFiveSecondsSinceLastCapture()
        {
            // Given
            var playerEntityId = 123L;
            var captures = 1;
            var showTenSecondCaptureMessage = false;
            var showFiveSecondCaptureMessage = false;
            var now = DateTime.Now;

            var escapeCaptureTime = now.AddSeconds(4);

            var playerEntity = new PlayerEntity
            {
                Id = playerEntityId,
                RawData = new ConcurrentDictionary<string, object>(),
            };
            playerEntity.SetProperty(
                GamePlayerCaptureState.PROPERTY_NAME,
                new GamePlayerCaptureState
                {
                    EscapeCaptureTime = escapeCaptureTime,
                    ShownTenSecondMessage = showTenSecondCaptureMessage,
                    ShownFiveSecondMessage = showFiveSecondCaptureMessage,
                    Captures = captures,
                }
            );

            var mediatorMock = new Mock<IMediator>();
            var dateTimeMock = new Mock<IDateTimeService>();

            mediatorMock.Setup(
                mock => mock.Send(
                    new FindPlayerByEntityId(
                        playerEntityId
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                playerEntity
            );

            dateTimeMock.Setup(
                mock => mock.Now
            ).Returns(
                now
            );

            // When
            var handler = new RunCaptureLogicForPlayerHandler(
                mediatorMock.Object,
                dateTimeMock.Object
            );
            await handler.Handle(
                new RunCaptureLogicForPlayer(
                    playerEntityId
                ),
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Send(
                    new ProcessFiveSecondCaptureLogic(
                        playerEntity
                    ),
                    CancellationToken.None
                )
            );
            mediatorMock.Verify(
                mock => mock.Send(
                    new ProcessTenSecondCaptureLogic(
                        playerEntity
                    ),
                    CancellationToken.None
                )
            );
        }

        [Fact]
        public async Task ShouldSendRunEscapeOfCapturesWhenLessThenZeroSecondsSinceLastCaptureAndCapturesGreaterThanZero()
        {
            // Given
            var playerEntityId = 123L;
            var captures = 1;
            var showTenSecondCaptureMessage = true;
            var showFiveSecondCaptureMessage = true;
            var now = DateTime.Now;

            var escapeCaptureTime = now.AddSeconds(-1);

            var playerEntity = new PlayerEntity
            {
                Id = playerEntityId,
                RawData = new ConcurrentDictionary<string, object>(),
            };
            playerEntity.SetProperty(
                GamePlayerCaptureState.PROPERTY_NAME,
                new GamePlayerCaptureState
                {
                    EscapeCaptureTime = escapeCaptureTime,
                    ShownTenSecondMessage = showTenSecondCaptureMessage,
                    ShownFiveSecondMessage = showFiveSecondCaptureMessage,
                    Captures = captures,
                }
            );

            var mediatorMock = new Mock<IMediator>();
            var dateTimeMock = new Mock<IDateTimeService>();

            mediatorMock.Setup(
                mock => mock.Send(
                    new FindPlayerByEntityId(
                        playerEntityId
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                playerEntity
            );

            dateTimeMock.Setup(
                mock => mock.Now
            ).Returns(
                now
            );

            // When
            var handler = new RunCaptureLogicForPlayerHandler(
                mediatorMock.Object,
                dateTimeMock.Object
            );
            await handler.Handle(
                new RunCaptureLogicForPlayer(
                    playerEntityId
                ),
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Send(
                    new RunEscapeOfCaptures(
                        playerEntity
                    ),
                    CancellationToken.None
                )
            );
            mediatorMock.Verify(
                mock => mock.Send(
                    new ProcessFiveSecondCaptureLogic(
                        playerEntity
                    ),
                    CancellationToken.None
                ),
                Times.Never()
            );
            mediatorMock.Verify(
                mock => mock.Send(
                    new ProcessTenSecondCaptureLogic(
                        playerEntity
                    ),
                    CancellationToken.None
                ),
                Times.Never()
            );
        }
    }
}
