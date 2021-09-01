namespace EventHorizon.Zone.System.Server.Scripts.Plugin.BackgroundTask.Tests.Remove
{
    using AutoFixture.Xunit2;

    using EventHorizon.Test.Common.Attributes;
    using EventHorizon.Zone.System.Server.Scripts.Plugin.BackgroundTask.Api;
    using EventHorizon.Zone.System.Server.Scripts.Plugin.BackgroundTask.Remove;

    using FluentAssertions;

    using global::System.Threading;
    using global::System.Threading.Tasks;

    using Moq;

    using Xunit;

    public class RemoveScriptedBackgroundTaskCommandHandlerTests
    {
        [Theory, AutoMoqData]
        public async Task ShouldCallRemoveOnRepositoryAndReturnSuccessResponseWhenCommandIsHandled(
            // Given
            [Frozen] Mock<BackgroundTaskWrapperRepository> repositoryMock,
            RemoveScriptedBackgroundTaskCommand request,
            RemoveScriptedBackgroundTaskCommandHandler handler
        )
        {
            // When
            var actual = await handler.Handle(
                request,
                CancellationToken.None
            );

            // Then
            actual.Success.Should().BeTrue();
            repositoryMock.Verify(
                mock => mock.TryRemove(
                    request.TaskId,
                    out It.Ref<BackgroundTaskWrapper>.IsAny
                )
            );
        }

        [Theory, AutoMoqData]
        public async Task ShouldCallDisposeOnRemovedTaskWrapperWhenRepositoryRemoveReturnNotNullTaskWrapper(
            // Given
            [Frozen] Mock<BackgroundTaskWrapper> backgroundTaskWrapperMock,
            [Frozen] Mock<BackgroundTaskWrapperRepository> repositoryMock,
            RemoveScriptedBackgroundTaskCommand request,
            RemoveScriptedBackgroundTaskCommandHandler handler
        )
        {
            var backgroundTaskWrapper = backgroundTaskWrapperMock.Object;
            repositoryMock.Setup(
                mock => mock.TryRemove(
                    request.TaskId,
                    out backgroundTaskWrapper
                )
            ).Returns(
                true
            );
            // When
            await handler.Handle(
                request,
                CancellationToken.None
            );

            // Then
            backgroundTaskWrapperMock.Verify(
                mock => mock.Dispose()
            );
        }
    }
}
