namespace EventHorizon.BackgroundTasks.Tests.State;

using System;
using System.Threading;
using System.Threading.Tasks;

using EventHorizon.BackgroundTasks.Model;
using EventHorizon.BackgroundTasks.State;
using EventHorizon.Test.Common.Attributes;

using FluentAssertions;

using Xunit;


public class StandardBackgroundJobsTests
{
    [Theory, AutoMoqData]
    public async Task DequeueAddedBackgroundTaskWhenQueued(
        // Given
        BackgroundTask backgroundTask,
        StandardBackgroundJobs standardBackgroundJobs
    )
    {
        // When
        await standardBackgroundJobs.QueueAsync(
            backgroundTask,
            CancellationToken.None
        );
        var actual = await standardBackgroundJobs.DequeueAsync(
            CancellationToken.None
        );

        // Then
        actual.Should().Be(backgroundTask);
    }

    [Theory, AutoMoqData]
    public async Task ThrowArgumentNullExceptionWhenBackgroundTaskQueued(
        // Given
        StandardBackgroundJobs standardBackgroundJobs
    )
    {
        // When
        Func<Task> action = async () => await standardBackgroundJobs.QueueAsync(
            null,
            CancellationToken.None
        );

        // Then
        await action.Should().ThrowAsync<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'workItem')");
    }
}
