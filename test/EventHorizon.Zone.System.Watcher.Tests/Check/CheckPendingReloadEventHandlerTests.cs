using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Admin.Plugin.Command.Events;
using EventHorizon.Zone.System.Admin.Plugin.Command.Model;
using EventHorizon.Zone.System.Watcher.Check;
using EventHorizon.Zone.System.Watcher.State;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EventHorizon.Zone.System.Watcher.Tests.Check
{
    public class CheckPendingReloadEventHandlerTests
    {
        [Fact]
        public async Task TestShouldPublishAdminCommandEventWhenReloadIsPending()
        {
            // Given
            var expected = "reload-system";

            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<CheckPendingReloadEventHandler>>();
            var pendingReloadMock = new Mock<PendingReloadState>();

            pendingReloadMock.Setup(
                mock => mock.IsPending
            ).Returns(
                true
            );

            // When
            var handler = new CheckPendingReloadEventHandler(
                mediatorMock.Object,
                loggerMock.Object,
                pendingReloadMock.Object
            );
            await handler.Handle(
                new CheckPendingReloadEvent(),
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Publish(
                    It.Is<AdminCommandEvent>(
                        command => command.Command.Command == expected
                    ),
                    CancellationToken.None
                )
            );
        }

        [Fact]
        public async Task TestShouldRemovePendingWhenReloadIsPending()
        {
            // Given
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<CheckPendingReloadEventHandler>>();
            var pendingReloadMock = new Mock<PendingReloadState>();

            pendingReloadMock.Setup(
                mock => mock.IsPending
            ).Returns(
                true
            );

            // When
            var handler = new CheckPendingReloadEventHandler(
                mediatorMock.Object,
                loggerMock.Object,
                pendingReloadMock.Object
            );
            await handler.Handle(
                new CheckPendingReloadEvent(),
                CancellationToken.None
            );

            // Then
            pendingReloadMock.Verify(
                mock => mock.RemovePending()
            );
        }

        [Fact]
        public async Task TestShouldNotRemovePendingWhenReloadIsNotPending()
        {
            // Given
            var expected = "reload-system";
            var expectedEvent = new AdminCommandEvent(
                It.Is<IAdminCommand>(
                    command => command.Command == expected
                ),
                null
            );

            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<CheckPendingReloadEventHandler>>();
            var pendingReloadMock = new Mock<PendingReloadState>();

            pendingReloadMock.Setup(
                mock => mock.IsPending
            ).Returns(
                false
            );

            // When
            var handler = new CheckPendingReloadEventHandler(
                mediatorMock.Object,
                loggerMock.Object,
                pendingReloadMock.Object
            );
            await handler.Handle(
                new CheckPendingReloadEvent(),
                CancellationToken.None
            );

            // Then
            pendingReloadMock.Verify(
                mock => mock.RemovePending(),
                Times.Never()
            );
        }

        [Fact]
        public async Task TestShouldNotPublishAnyEventsWhenReloadIsNotPending()
        {
            // Given
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<CheckPendingReloadEventHandler>>();
            var pendingReloadMock = new Mock<PendingReloadState>();

            pendingReloadMock.Setup(
                mock => mock.IsPending
            ).Returns(
                false
            );

            // When
            var handler = new CheckPendingReloadEventHandler(
                mediatorMock.Object,
                loggerMock.Object,
                pendingReloadMock.Object
            );
            await handler.Handle(
                new CheckPendingReloadEvent(),
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
    }
}