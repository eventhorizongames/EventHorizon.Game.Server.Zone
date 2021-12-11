namespace EventHorizon.Zone.System.ArtifactManagement.Tests.Tasks;

using AutoFixture.Xunit2;

using EventHorizon.Test.Common.Attributes;
using EventHorizon.Zone.Core.Model.Command;
using EventHorizon.Zone.System;

using global::System.Threading.Tasks;
using global::System.Threading;
using MediatR;
using Moq;
using Xunit;
using EventHorizon.Zone.System.ArtifactManagement.Tasks;
using EventHorizon.Zone.System.ArtifactManagement.ClientActions;
using EventHorizon.Zone.System.ArtifactManagement.Import;
using FluentAssertions;

public class ImportZoneDataTaskHandlerTests
{
    [Theory, AutoMoqData]
    public async Task NotifiyFinishedZoneServerImportWhenImportZoneDataIsSuccessful(
        // Given
        ImportZoneDataTask task,
        [Frozen] Mock<ISender> senderMock,
        [Frozen] Mock<IPublisher> publisherMock,
        ImportZoneDataTaskHandler handler
    )
    {
        var expected = AdminClientActionFinishedZoneServerImportEvent.Create(
            task.ReferenceId
        );

        senderMock.Setup(
            mock => mock.Send(
                new ImportZoneDataCommand(
                    task.ReferenceId,
                    task.ImportArtifactUrl
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
    public async Task NotifiyFinishedZoneServerImportClientActionWhenImportZoneDataIsSuccessful(
        // Given
        ImportZoneDataTask task,
        [Frozen] Mock<ISender> senderMock,
        [Frozen] Mock<IPublisher> publisherMock,
        ImportZoneDataTaskHandler handler
    )
    {
        var expected = ClientActionFinishedZoneServerImportEvent.Create(
            task.ReferenceId
        );

        senderMock.Setup(
            mock => mock.Send(
                new ImportZoneDataCommand(
                    task.ReferenceId,
                    task.ImportArtifactUrl
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
    public async Task NotifiyFailedZoneServerImportWhenImportZoneDataFailed(
        // Given
        ImportZoneDataTask task,
        string errorCode,
        [Frozen] Mock<ISender> senderMock,
        [Frozen] Mock<IPublisher> publisherMock,
        ImportZoneDataTaskHandler handler
    )
    {
        var expected = AdminClientActionFailedZoneServerImportEvent.Create(
            task.ReferenceId,
            errorCode
        );

        senderMock.Setup(
            mock => mock.Send(
                new ImportZoneDataCommand(
                    task.ReferenceId,
                    task.ImportArtifactUrl
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
