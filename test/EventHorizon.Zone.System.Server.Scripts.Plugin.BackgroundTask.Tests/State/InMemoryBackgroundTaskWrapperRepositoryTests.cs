namespace EventHorizon.Zone.System.Server.Scripts.Plugin.BackgroundTask.Tests.State;

using EventHorizon.Test.Common.Attributes;
using EventHorizon.Zone.System.Server.Scripts.Plugin.BackgroundTask.Api;
using EventHorizon.Zone.System.Server.Scripts.Plugin.BackgroundTask.State;

using FluentAssertions;

using Xunit;

public class InMemoryBackgroundTaskWrapperRepositoryTests
{
    [Theory, AutoMoqData]
    public void ShoulGetBackgroundTaskWrapperBackAddedWhenAddedToRepository(
        // Given
        string taskId,
        BackgroundTaskWrapper wrapper,
        InMemoryBackgroundTaskWrapperRepository repository
    )
    {
        // When
        repository.Add(
            taskId,
            wrapper
        );
        var isFound = repository.TryGet(
            taskId,
            out var actual
        );

        // Then
        isFound.Should().BeTrue();
        actual.Should().Be(wrapper);
    }

    [Theory, AutoMoqData]
    public void ShouldNotReplaceWrapperByIdWhenAnotherWrapperWasAlreadyAdded(
        // Given
        string taskId,
        BackgroundTaskWrapper wrapper,
        BackgroundTaskWrapper otherWrapper,
        InMemoryBackgroundTaskWrapperRepository repository
    )
    {
        // When
        repository.Add(
            taskId,
            wrapper
        );
        repository.Add(
            taskId,
            otherWrapper
        );
        repository.TryGet(
            taskId,
            out var actual
        );

        // Then
        actual.Should().Be(wrapper)
            .And.NotBe(otherWrapper);
    }

    [Theory, AutoMoqData]
    public void ShouldAllowForNewWrapperToBeAddedWhenRemovedBeforeAdd(
        // Given
        string taskId,
        BackgroundTaskWrapper wrapper,
        BackgroundTaskWrapper otherWrapper,
        InMemoryBackgroundTaskWrapperRepository repository
    )
    {
        // When
        repository.Add(
            taskId,
            wrapper
        );
        repository.TryRemove(
            taskId,
            out var _
        );
        repository.Add(
            taskId,
            otherWrapper
        );
        repository.TryGet(
            taskId,
            out var actual
        );

        // Then
        actual.Should().Be(otherWrapper)
            .And.NotBe(wrapper);
    }

    [Theory, AutoMoqData]
    public void ShouldReturnTheExistingWrapperOnRemoveWhenRepositoryContainsWrapper(
        // Given
        string taskId,
        BackgroundTaskWrapper wrapper,
        InMemoryBackgroundTaskWrapperRepository repository
    )
    {
        // When
        repository.Add(
            taskId,
            wrapper
        );
        repository.TryRemove(
            taskId,
            out var actual
        );

        // Then
        actual.Should().Be(wrapper);
    }

    [Theory, AutoMoqData]
    public void ShouldReturnNullWrapperWhenRepositoryDoesNotContainWrapper(
        // Given
        string taskId,
        InMemoryBackgroundTaskWrapperRepository repository
    )
    {
        // When
        var isFound = repository.TryGet(
            taskId,
            out var actual
        );

        // Then
        isFound.Should().BeFalse();
        actual.Should().BeNull();
    }
}
