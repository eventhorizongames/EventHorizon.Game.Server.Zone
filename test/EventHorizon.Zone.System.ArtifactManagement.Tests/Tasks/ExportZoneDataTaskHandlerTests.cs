namespace EventHorizon.Zone.System.ArtifactManagement.Tests.Tasks;

using AutoFixture.Xunit2;

using EventHorizon.Test.Common.Attributes;
using EventHorizon.Zone.Core.Model.Command;
using EventHorizon.Zone.System.ArtifactManagement.ClientActions;
using EventHorizon.Zone.System.ArtifactManagement.Export;
using EventHorizon.Zone.System.ArtifactManagement.Tasks;

using FluentAssertions;

using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

using Moq;

using Xunit;

public class ExportZoneDataTaskHandlerTests
{
    [Theory, AutoMoqData]
    public async Task NotifiyFinishedZoneServerExportWhenExportZoneDataIsSuccessful(
        // Given
        ExportZoneDataTask task,
        string path,
        [Frozen] Mock<ISender> senderMock,
        [Frozen] Mock<IPublisher> publisherMock,
        ExportZoneDataTaskHandler handler
    )
    {
        var expected = AdminClientActionFinishedZoneServerExportEvent.Create(
            task.ReferenceId,
            path
        );

        senderMock.Setup(
            mock => mock.Send(
                new ExportZoneDataCommand(
                    task.ReferenceId
                ),
                CancellationToken.None
            )
        ).ReturnsAsync(
            new CommandResult<ExportZoneDataResult>(
                new ExportZoneDataResult(
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
    public async Task NotifiyFailedZoneServerExportWhenExportZoneDataFailed(
        // Given
        ExportZoneDataTask task,
        string errorCode,
        [Frozen] Mock<ISender> senderMock,
        [Frozen] Mock<IPublisher> publisherMock,
        ExportZoneDataTaskHandler handler
    )
    {
        var expected = AdminClientActionFailedZoneServerExportEvent.Create(
            task.ReferenceId,
            errorCode
        );

        senderMock.Setup(
            mock => mock.Send(
                new ExportZoneDataCommand(
                    task.ReferenceId
                ),
                CancellationToken.None
            )
        ).ReturnsAsync(
            new CommandResult<ExportZoneDataResult>(
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
