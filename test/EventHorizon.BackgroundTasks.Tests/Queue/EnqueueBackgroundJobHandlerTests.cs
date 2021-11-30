namespace EventHorizon.BackgroundTasks.Tests.Queue;

using System;
using System.Threading;
using System.Threading.Tasks;

using AutoFixture.Xunit2;

using EventHorizon.BackgroundTasks.Api;
using EventHorizon.BackgroundTasks.Queue;
using EventHorizon.Test.Common.Attributes;

using FluentAssertions;

using Moq;

using Xunit;


public class EnqueueBackgroundJobHandlerTests
{
    [Theory, AutoMoqData]
    public async Task QueueBackgroundWorkWhenCommandIsHandled(
        // Given
        EnqueueBackgroundJob backgroundJob,
        [Frozen] Mock<BackgroundJobs> backgroundJobsMock,
        EnqueueBackgroundJobHandler handler
    )
    {
        // When
        var actual = await handler.Handle(
            backgroundJob,
            CancellationToken.None
        );

        // Then
        actual.Success.Should().BeTrue();
        backgroundJobsMock.Verify(
            mock => mock.QueueAsync(
                backgroundJob.Task,
                It.IsAny<CancellationToken>()
            )
        );
    }
}
