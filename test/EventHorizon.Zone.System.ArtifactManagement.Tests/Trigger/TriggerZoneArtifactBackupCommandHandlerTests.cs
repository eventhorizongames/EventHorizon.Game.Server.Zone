namespace EventHorizon.Zone.System.ArtifactManagement.Tests.Trigger;

using AutoFixture.Xunit2;

using EventHorizon.BackgroundTasks.Queue;
using EventHorizon.Test.Common.Attributes;
using EventHorizon.Zone.System.ArtifactManagement.Tasks;
using EventHorizon.Zone.System.ArtifactManagement.Trigger;

using FluentAssertions;

using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

using Moq;

using Xunit;

public class TriggerZoneArtifactBackupCommandHandlerTests
{
    [Theory, AutoMoqData]
    public async Task EnequeBackgroundJobWhenCommandIsHandled(
        // Given
        [Frozen] Mock<ISender> senderMock,
        TriggerZoneArtifactBackupCommandHandler handler
    )
    {
        // When
        var actual = await handler.Handle(
            new TriggerZoneArtifactBackupCommand(),
            CancellationToken.None
        );

        // Then
        actual.Success.Should().BeTrue();

        senderMock.Verify(
            mock => mock.Send(
                It.Is<EnqueueBackgroundJob>(
                    a =>
                        a.Task.GetType().Equals(typeof(BackupZoneDataTask))
                ),
                CancellationToken.None
            )
        );
    }
}
