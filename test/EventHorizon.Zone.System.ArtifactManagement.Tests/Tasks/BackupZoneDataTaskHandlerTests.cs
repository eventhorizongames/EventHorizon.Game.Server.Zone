namespace EventHorizon.Zone.System.ArtifactManagement.Tests.Tasks;

using AutoFixture.Xunit2;

using EventHorizon.Test.Common.Attributes;
using EventHorizon.Zone.Core.Model.Command;
using EventHorizon.Zone.System.ArtifactManagement.Backup;
using EventHorizon.Zone.System.ArtifactManagement.ClientActions;
using EventHorizon.Zone.System.ArtifactManagement.Tasks;

using FluentAssertions;

using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

using Moq;

using Xunit;

public class BackupZoneDataTaskHandlerTests
{
    [Theory, AutoMoqData]
    public async Task NotifiyFinishedZoneServerBackupWhenBackupZoneDataIsSuccessful(
        // Given
        BackupZoneDataTask task,
        string path,
        [Frozen] Mock<ISender> senderMock,
        [Frozen] Mock<IPublisher> publisherMock,
        BackupZoneDataTaskHandler handler
    )
    {
        var expected = AdminClientActionFinishedZoneServerBackupEvent.Create(
            task.ReferenceId,
            path
        );

        senderMock.Setup(
            mock => mock.Send(
                new BackupZoneDataCommand(
                    task.ReferenceId
                ),
                CancellationToken.None
            )
        ).ReturnsAsync(
            new CommandResult<BackupZoneDataResult>(
                new BackupZoneDataResult(
                    task.ReferenceId,
                    path
                )
            )
        );

        // When
        var actual = await handler.Handle(
            task,
            CancellationToken.None
        );

        // Then
        actual.Success.Should().BeTrue();
        publisherMock.Verify(
            mock => mock.Publish(
                expected,
                CancellationToken.None
            )
        );
    }

    [Theory, AutoMoqData]
    public async Task NotifiyFailedZoneServerBackupWhenBackupZoneDataFailed(
        // Given
        BackupZoneDataTask task,
        string errorCode,
        [Frozen] Mock<ISender> senderMock,
        [Frozen] Mock<IPublisher> publisherMock,
        BackupZoneDataTaskHandler handler
    )
    {
        var expected = AdminClientActionFailedZoneServerBackupEvent.Create(
            task.ReferenceId,
            errorCode
        );

        senderMock.Setup(
            mock => mock.Send(
                new BackupZoneDataCommand(
                    task.ReferenceId
                ),
                CancellationToken.None
            )
        ).ReturnsAsync(
            new CommandResult<BackupZoneDataResult>(
                errorCode
            )
        );

        // When
        var actual = await handler.Handle(
            task,
            CancellationToken.None
        );

        // Then
        actual.Success.Should().BeFalse();
        publisherMock.Verify(
            mock => mock.Publish(
                expected,
                CancellationToken.None
            )
        );
    }
}
