namespace EventHorizon.Zone.System.ArtifactManagement.Tests.Revert;

using AutoFixture.Xunit2;

using EventHorizon.Test.Common.Attributes;
using EventHorizon.Zone.Core.Events.DirectoryService;
using EventHorizon.Zone.Core.Events.FileService;
using EventHorizon.Zone.Core.Model.Command;
using EventHorizon.Zone.Core.Model.DirectoryService;
using EventHorizon.Zone.Core.Model.FileService;
using EventHorizon.Zone.System.Admin.Restart;
using EventHorizon.Zone.System.ArtifactManagement.Query;
using EventHorizon.Zone.System.ArtifactManagement.Revert;

using FluentAssertions;

using global::System;
using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

using Moq;

using Xunit;

public class RevertToBackupZoneDataCommandHandlerTests
{
    [Theory, AutoMoqData]
    public async Task ReturnSuccessWhenCommandIsHandled(
        // Given
        string referenceId,
        string backupArtifactUrl,
        [Frozen] Mock<ISender> senderMock,
        RevertToBackupZoneDataCommandHandler handler
    )
    {
        senderMock.GivenValidSetup();

        // When
        var actual = await handler.Handle(
            new RevertToBackupZoneDataCommand(
                referenceId,
                backupArtifactUrl
            ),
            CancellationToken.None
        );

        // Then
        actual.Success.Should().BeTrue();
    }

    [Theory, AutoMoqData]
    public async Task ReturnFailureWhenImportArtifactIsNotValid(
        // Given
        string referenceId,
        string backupArtifactUrl,
        [Frozen] Mock<ISender> senderMock,
        RevertToBackupZoneDataCommandHandler handler
    )
    {
        var expected = "ARTIFACT_MANAGEMENT_REVERT_TO_BACKUP_STEP_DOMAIN_VALIDATION_FAILED";
        senderMock.GivenValidSetup();

        senderMock.Setup(
            mock => mock.Send(
                new IsNotValidArtifactUrlDomain(
                    backupArtifactUrl
                ),
                CancellationToken.None
            )
        ).ReturnsAsync(
            true
        );

        // When
        var actual = await handler.Handle(
            new RevertToBackupZoneDataCommand(
                referenceId,
                backupArtifactUrl
            ),
            CancellationToken.None
        );

        // Then
        actual.Success.Should().BeFalse();
        actual.ErrorCode.Should().Be(
            expected
        );
    }

    [Theory, AutoMoqData]
    public async Task ReturnErrorWhenDownloadFileFails(
        // Given
        string referenceId,
        Uri backupArtifactUri,
        [Frozen] Mock<ISender> senderMock,
        RevertToBackupZoneDataCommandHandler handler
    )
    {
        var expected = "ARTIFACT_MANAGEMENT_REVERT_TO_BACKUP_STEP_DOWNLOAD_ARTIFACT_FAILED";

        senderMock.GivenValidSetup();

        senderMock.Setup(
            mock => mock.Send(
                It.IsAny<DownloadFileFromRemoteUrlCommand>(),
                CancellationToken.None
            )
        ).ReturnsAsync(
            new CommandResult<DownloadFileFromRemoteUrlResult>(
                "Failed"
            )
        );

        // When
        var actual = await handler.Handle(
            new RevertToBackupZoneDataCommand(
                referenceId,
                backupArtifactUri.ToString()
            ),
            CancellationToken.None
        );

        // Then
        actual.Success.Should().BeFalse();
        actual.ErrorCode.Should().Be(expected);
    }

    [Theory, AutoMoqData]
    public async Task ReturnErrorWhenDeleteAppDataPathFails(
        // Given
        string referenceId,
        Uri importArtifactUri,
        [Frozen] Mock<ISender> senderMock,
        RevertToBackupZoneDataCommandHandler handler
    )
    {
        var expected = "ARTIFACT_MANAGEMENT_REVERT_TO_BACKUP_STEP_DELETE_DATA_FAILED";

        senderMock.GivenValidSetup();

        senderMock.Setup(
            mock => mock.Send(
                It.IsAny<DeleteDirectoryRecursivelyCommand>(),
                CancellationToken.None
            )
        ).ReturnsAsync(
            new StandardCommandResult(
                "Failed"
            )
        );

        // When
        var actual = await handler.Handle(
            new RevertToBackupZoneDataCommand(
                referenceId,
                importArtifactUri.ToString()
            ),
            CancellationToken.None
        );

        // Then
        actual.Success.Should().BeFalse();
        actual.ErrorCode.Should().Be(expected);
    }

    [Theory, AutoMoqData]
    public async Task ReturnErrorWhenExtractArtifactFails(
        // Given
        string referenceId,
        Uri backupArtifactUri,
        [Frozen] Mock<ISender> senderMock,
        RevertToBackupZoneDataCommandHandler handler
    )
    {
        var expected = "ARTIFACT_MANAGEMENT_REVERT_TO_BACKUP_STEP_EXTRACT_FAILED";

        senderMock.GivenValidSetup();

        senderMock.Setup(
            mock => mock.Send(
                It.IsAny<ExtractArtifactIntoDirectoryCommand>(),
                CancellationToken.None
            )
        ).ReturnsAsync(
            new CommandResult<StandardDirectoryInfo>(
                "Failed"
            )
        );

        // When
        var actual = await handler.Handle(
            new RevertToBackupZoneDataCommand(
                referenceId,
                backupArtifactUri.ToString()
            ),
            CancellationToken.None
        );

        // Then
        actual.Success.Should().BeFalse();
        actual.ErrorCode.Should().Be(expected);
    }

    [Theory, AutoMoqData]
    public async Task ReturnErrorWhenRestartServerFails(
        // Given
        string referenceId,
        Uri backupArtifactUri,
        [Frozen] Mock<ISender> senderMock,
        RevertToBackupZoneDataCommandHandler handler
    )
    {
        var expected = "ARTIFACT_MANAGEMENT_REVERT_TO_BACKUP_STEP_RESTART_SERVER_FAILED";

        senderMock.GivenValidSetup();

        senderMock.Setup(
            mock => mock.Send(
                It.IsAny<RestartServerCommand>(),
                CancellationToken.None
            )
        ).ReturnsAsync(
            new StandardCommandResult(
                "Failed"
            )
        );

        // When
        var actual = await handler.Handle(
            new RevertToBackupZoneDataCommand(
                referenceId,
                backupArtifactUri.ToString()
            ),
            CancellationToken.None
        );

        // Then
        actual.Success.Should().BeFalse();
        actual.ErrorCode.Should().Be(expected);
    }

    [Theory, AutoMoqData]
    public async Task ReturnErrorWhenUnmanagedErrorIsThrown(
        // Given
        string referenceId,
        Uri backupArtifactUri,
        [Frozen] Mock<ISender> senderMock,
        RevertToBackupZoneDataCommandHandler handler
    )
    {
        var expected = "ARTIFACT_MANAGEMENT_REVERT_TO_BACKUP_EXCEPTION";

        senderMock.Setup(
            mock => mock.Send(
                It.IsAny<IsNotValidArtifactUrlDomain>(),
                CancellationToken.None
            )
        ).ThrowsAsync(
            new Exception("I am unmanaged!")
        );

        // When
        var actual = await handler.Handle(
            new RevertToBackupZoneDataCommand(
                referenceId,
                backupArtifactUri.ToString()
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
        this Mock<ISender> senderMock
    )
    {
        senderMock.Setup(
            mock => mock.Send(
                It.IsAny<IsNotValidArtifactUrlDomain>(),
                CancellationToken.None
            )
        ).ReturnsAsync(
            false
        );

        var fileInfo = new StandardFileInfo(
            "name",
            "directory-name",
            "full-name",
            "extension"
        );

        senderMock.Setup(
            mock => mock.Send(
                It.IsAny<DownloadFileFromRemoteUrlCommand>(),
                It.IsAny<CancellationToken>()
            )
        ).ReturnsAsync(
            new CommandResult<DownloadFileFromRemoteUrlResult>(
                new DownloadFileFromRemoteUrlResult(
                    fileInfo
                )
            )
        );

        senderMock.Setup(
            mock => mock.Send(
                It.IsAny<DeleteDirectoryRecursivelyCommand>(),
                It.IsAny<CancellationToken>()
            )
        ).ReturnsAsync(
            new StandardCommandResult()
        );

        senderMock.Setup(
            mock => mock.Send(
                It.IsAny<ExtractArtifactIntoDirectoryCommand>(),
                It.IsAny<CancellationToken>()
            )
        ).ReturnsAsync(
            new CommandResult<StandardDirectoryInfo>(
                new StandardDirectoryInfo(
                    "name",
                    "full-name",
                    "parent-full-name"
                )
            )
        );
    }
}
