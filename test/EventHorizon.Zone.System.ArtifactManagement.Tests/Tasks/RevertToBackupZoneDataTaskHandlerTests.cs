namespace EventHorizon.Zone.System.ArtifactManagement.Tests.Tasks;

using AutoFixture.Xunit2;

using EventHorizon.Test.Common.Attributes;
using EventHorizon.Zone.Core.Model.Command;
using EventHorizon.Zone.System.ArtifactManagement.ClientActions;
using EventHorizon.Zone.System.ArtifactManagement.Revert;
using EventHorizon.Zone.System.ArtifactManagement.Tasks;

using FluentAssertions;

using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

using Moq;

using Xunit;

public class RevertToBackupZoneDataTaskHandlerTests
{
    [Theory, AutoMoqData]
    public async Task NotifiyFinishedZoneServerRevertToBackupWhenRevertToBackupZoneDataIsSuccessful(
        // Given
        RevertToBackupZoneDataTask task,
        [Frozen] Mock<ISender> senderMock,
        [Frozen] Mock<IPublisher> publisherMock,
        RevertToBackupZoneDataTaskHandler handler
    )
    {
        var expected = AdminClientActionFinishedZoneServerRevertToBackupEvent.Create(
            task.ReferenceId
        );

        senderMock.Setup(
            mock => mock.Send(
                new RevertToBackupZoneDataCommand(
                    task.ReferenceId,
                    task.BackupArtifactUrl
                ),
                CancellationToken.None
            )
        ).ReturnsAsync(
            new StandardCommandResult()
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
    public async Task NotifiyFinishedZoneServerRevertToBackupClientActionWhenRevertToBackupZoneDataIsSuccessful(
        // Given
        RevertToBackupZoneDataTask task,
        [Frozen] Mock<ISender> senderMock,
        [Frozen] Mock<IPublisher> publisherMock,
        RevertToBackupZoneDataTaskHandler handler
    )
    {
        var expected = ClientActionFinishedZoneServerRevertToBackupEvent.Create(
            task.ReferenceId
        );

        senderMock.Setup(
            mock => mock.Send(
                new RevertToBackupZoneDataCommand(
                    task.ReferenceId,
                    task.BackupArtifactUrl
                ),
                CancellationToken.None
            )
        ).ReturnsAsync(
            new StandardCommandResult()
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
    public async Task NotifiyFailedZoneServerRevertToBackupWhenRevertToBackupZoneDataFailed(
        // Given
        RevertToBackupZoneDataTask task,
        string errorCode,
        [Frozen] Mock<ISender> senderMock,
        [Frozen] Mock<IPublisher> publisherMock,
        RevertToBackupZoneDataTaskHandler handler
    )
    {
        var expected = AdminClientActionFailedZoneServerRevertToBackupEvent.Create(
            task.ReferenceId,
            errorCode
        );

        senderMock.Setup(
            mock => mock.Send(
                new RevertToBackupZoneDataCommand(
                    task.ReferenceId,
                    task.BackupArtifactUrl
                ),
                CancellationToken.None
            )
        ).ReturnsAsync(
            new StandardCommandResult(
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
