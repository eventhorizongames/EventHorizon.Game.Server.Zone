namespace EventHorizon.Zone.System.ArtifactManagement.Tests.Export;

using AutoFixture.Xunit2;

using EventHorizon.Test.Common.Attributes;
using EventHorizon.Zone.Core.Events.DirectoryService;
using EventHorizon.Zone.Core.Events.FileService;
using EventHorizon.Zone.Core.Model.Command;
using EventHorizon.Zone.Core.Model.DateTimeService;
using EventHorizon.Zone.Core.Model.FileService;
using EventHorizon.Zone.System;
using EventHorizon.Zone.System.ArtifactManagement.Backup;
using EventHorizon.Zone.System.ArtifactManagement.Export;
using EventHorizon.Zone.System.AssetServer.Backup;
using EventHorizon.Zone.System.AssetServer.Export;

using FluentAssertions;

using global::System;
using global::System.Collections.Generic;
using global::System.IO;
using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

using Moq;

using Xunit;

public class ExportZoneDataCommandHandlerTests
{
    [Theory, AutoMoqData]
    public async Task VerifyExportIsUploadedWhenCommandIsHandled(
        // Given
        string referenceId,
        [Frozen] Mock<ISender> senderMock,
        [Frozen] Mock<IDateTimeService> dateTimeServiceMock,
        ExportZoneDataCommandHandler handler
    )
    {
        var now = DateTime.Now;
        var memoryStream = new MemoryStream();
        senderMock.GivenValidSetup(
            memoryStream
        );

        var expectedFileName = $"export.{now.Ticks}.{referenceId}.zip";

        dateTimeServiceMock.Setup(
            mock => mock.Now
        ).Returns(
            now
        );

        // When
        var actual = await handler.Handle(
            new ExportZoneDataCommand(
                referenceId
            ),
            CancellationToken.None
        );

        // Then
        actual.Success.Should().BeTrue();
        senderMock.Verify(
            mock => mock.Send(
                new UploadAssetServerExportArtifactCommand(
                    "zone",
                    expectedFileName,
                    memoryStream
                ),
                CancellationToken.None
            )
        );
    }

    [Theory, AutoMoqData]
    public async Task ReturnErrorCodeWhenDirectoryForDesiniationFails(
        // Given
        string referenceId,
        [Frozen] Mock<ISender> senderMock,
        ExportZoneDataCommandHandler handler
    )
    {
        var expected = "ARTIFACT_MANAGEMENT_EXPORT_STEP_CREATE_DESTINATION_FAILED";

        senderMock.GivenValidSetup();

        senderMock.Setup(
            mock => mock.Send(
                It.IsAny<CreateDirectory>(),
                CancellationToken.None
            )
        ).ReturnsAsync(
            false
        );

        // When
        var actual = await handler.Handle(
            new ExportZoneDataCommand(
                referenceId
            ),
            CancellationToken.None
        );

        // Then
        actual.Success.Should().BeFalse();
        actual.ErrorCode.Should().Be(expected);
    }

    [Theory, AutoMoqData]
    public async Task ReturnErrorCodeWhenExistingExportFailsToDelete(
        // Given
        string referenceId,
        [Frozen] Mock<ISender> senderMock,
        ExportZoneDataCommandHandler handler
    )
    {
        var expected = "ARTIFACT_MANAGEMENT_EXPORT_STEP_DELETE_EXISTING_EXPORTS_FAILED";

        senderMock.GivenValidSetup();

        senderMock.Setup(
            mock => mock.Send(
                It.IsAny<DeleteFile>(),
                CancellationToken.None
            )
        ).ThrowsAsync(
            new Exception("Failed")
        );

        // When
        var actual = await handler.Handle(
            new ExportZoneDataCommand(
                referenceId
            ),
            CancellationToken.None
        );

        // Then
        actual.Success.Should().BeFalse();
        actual.ErrorCode.Should().Be(expected);
    }

    [Theory, AutoMoqData]
    public async Task ReturnErrorCodeWhenCreateArtifactFails(
        // Given
        string referenceId,
        [Frozen] Mock<ISender> senderMock,
        ExportZoneDataCommandHandler handler
    )
    {
        var expected = "ARTIFACT_MANAGEMENT_EXPORT_STEP_CREATE_DIRECTORY_FAILED";

        senderMock.GivenValidSetup();

        senderMock.Setup(
            mock => mock.Send(
                It.IsAny<CreateArtifactFromDirectoryCommand>(),
                CancellationToken.None
            )
        ).ReturnsAsync(
            "error-code"
        );

        // When
        var actual = await handler.Handle(
            new ExportZoneDataCommand(
                referenceId
            ),
            CancellationToken.None
        );

        // Then
        actual.Success.Should().BeFalse();
        actual.ErrorCode.Should().Be(expected);
    }

    [Theory, AutoMoqData]
    public async Task ReturnErrorCodeWhenUploadArtifactFails(
        // Given
        string referenceId,
        [Frozen] Mock<ISender> senderMock,
        ExportZoneDataCommandHandler handler
    )
    {
        var expected = "ARTIFACT_MANAGEMENT_EXPORT_ARTIFACT_UPLOAD_FAILED";

        senderMock.GivenValidSetup();

        senderMock.Setup(
            mock => mock.Send(
                It.IsAny<UploadAssetServerExportArtifactCommand>(),
                CancellationToken.None
            )
        ).ReturnsAsync(
            "error-code"
        );

        // When
        var actual = await handler.Handle(
            new ExportZoneDataCommand(
                referenceId
            ),
            CancellationToken.None
        );

        // Then
        actual.Success.Should().BeFalse();
        actual.ErrorCode.Should().Be(expected);
    }
}

public static class SenderMockExtensions
{
    public static void GivenValidSetup(
        this Mock<ISender> senderMock,
        Stream content = null
    )
    {
        if (content is null)
        {
            content = new MemoryStream();
        }

        senderMock.Setup(
            mock => mock.Send(
                It.IsAny<CreateDirectory>(),
                It.IsAny<CancellationToken>()
            )
        ).ReturnsAsync(
            true
        );

        senderMock.Setup(
            mock => mock.Send(
                It.IsAny<GetListOfFilesFromDirectory>(),
                It.IsAny<CancellationToken>()
            )
        ).ReturnsAsync(
            new List<StandardFileInfo>
            {
                new StandardFileInfo(
                    "name",
                    "directory-name",
                    "full-name",
                    "extensions"
                )
            }
        );

        senderMock.Setup(
            mock => mock.Send(
                It.IsAny<CreateArtifactFromDirectoryCommand>(),
                It.IsAny<CancellationToken>()
            )
        ).ReturnsAsync(
            new CommandResult<StandardFileInfo>(
                new StandardFileInfo(
                    "name",
                    "directory-name",
                    "full-name",
                    "extensions"
                )
            )
        );

        senderMock.Setup(
            mock => mock.Send(
                It.IsAny<GetStreamForFileInfo>(),
                It.IsAny<CancellationToken>()
            )
        ).ReturnsAsync(
            content
        );

        senderMock.Setup(
            mock => mock.Send(
                It.IsAny<UploadAssetServerExportArtifactCommand>(),
                It.IsAny<CancellationToken>()
            )
        ).ReturnsAsync(
            new CommandResult<UploadAssetServerExportArtifactResult>(
                new UploadAssetServerExportArtifactResult(
                    "service",
                    "path"
                )
            )
        );
    }
}
