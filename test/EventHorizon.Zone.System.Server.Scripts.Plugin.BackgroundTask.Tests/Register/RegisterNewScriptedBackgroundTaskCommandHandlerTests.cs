namespace EventHorizon.Zone.System.Server.Scripts.Plugin.BackgroundTask.Tests.Register;

using AutoFixture.Xunit2;

using EventHorizon.Test.Common.Attributes;
using EventHorizon.Zone.System.Server.Scripts.Plugin.BackgroundTask.Api;
using EventHorizon.Zone.System.Server.Scripts.Plugin.BackgroundTask.Model;
using EventHorizon.Zone.System.Server.Scripts.Plugin.BackgroundTask.Register;

using FluentAssertions;

using global::System.Threading;
using global::System.Threading.Tasks;

using Moq;

using Xunit;

public class RegisterNewScriptedBackgroundTaskCommandHandlerTests
{
    [Theory, AutoMoqData]
    public async Task ShouldAddTaskWrapperToRepositoryWhenUsingBuilder(
        // Given
        [Frozen] ScriptedBackgroundTask backgroundTask,
        [Frozen] BackgroundTaskWrapper backgroundTaskWrapper,
        [Frozen] Mock<BackgroundTaskWrapperRepository> repositoryMock,
        RegisterNewScriptedBackgroundTaskCommand request,
        RegisterNewScriptedBackgroundTaskCommandHandler handler
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
            mock => mock.Add(
                backgroundTask.Id,
                backgroundTaskWrapper
            )
        );
    }

    [Theory, AutoMoqData]
    public async Task ShouldDisposeOfTaskWrapperWhenRespositoryReturnsTrueOnRemove(
        // Given
        [Frozen] ScriptedBackgroundTask backgroundTask,
        [Frozen] Mock<BackgroundTaskWrapper> backgroundTaskWrapperMock,
        [Frozen] Mock<BackgroundTaskWrapperRepository> repositoryMock,
        RegisterNewScriptedBackgroundTaskCommand request,
        RegisterNewScriptedBackgroundTaskCommandHandler handler
    )
    {
        var backgroundTaskWrapper = backgroundTaskWrapperMock.Object;
        repositoryMock.Setup(
            mock => mock.TryRemove(
                backgroundTask.Id,
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

    [Theory, AutoMoqData]
    public async Task ShouldStartBuiltWhenRequestIsHandled(
        // Given
        [Frozen] Mock<BackgroundTaskWrapper> backgroundTaskWrapperMock,
        RegisterNewScriptedBackgroundTaskCommand request,
        RegisterNewScriptedBackgroundTaskCommandHandler handler
    )
    {
        // When
        await handler.Handle(
            request,
            CancellationToken.None
        );

        // Then
        backgroundTaskWrapperMock.Verify(
            mock => mock.Start()
        );
    }
}
