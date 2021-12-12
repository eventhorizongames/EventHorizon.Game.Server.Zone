namespace EventHorizon.Zone.System.ArtifactManagement.Tests.Import;

using AutoFixture.Xunit2;

using EventHorizon.Test.Common.Attributes;
using EventHorizon.Zone.Core.Events.DirectoryService;
using EventHorizon.Zone.Core.Events.FileService;
using EventHorizon.Zone.Core.Events.Lifetime;
using EventHorizon.Zone.Core.Model.Command;
using EventHorizon.Zone.Core.Model.DirectoryService;
using EventHorizon.Zone.Core.Model.FileService;
using EventHorizon.Zone.System.Admin.Restart;
using EventHorizon.Zone.System.ArtifactManagement.Backup;
using EventHorizon.Zone.System.ArtifactManagement.Import;
using EventHorizon.Zone.System.ArtifactManagement.Query;
using EventHorizon.Zone.System.ArtifactManagement.Trigger;

using FluentAssertions;

using global::System;
using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

using Moq;

using Xunit;

public class ImportZoneDataCommandHandlerTests
{
    [Theory]
    [InlineAutoMoqData("https://valid-domain.com/file.zip")]
    public async Task VerifyBackupIsCreatedWhenCommandIsHandled(
        // Given
        string referenceId,
        Uri importArtifactUrl,
        [Frozen] Mock<ISender> senderMock,
        ImportZoneDataCommandHandler handler
    )
    {
        senderMock.GivenValidSetup();

        // When
        var actual = await handler.Handle(
            new ImportZoneDataCommand(
                referenceId,
                importArtifactUrl.ToString()
            ),
            CancellationToken.None
        );

        // Then
        actual.Success.Should().BeTrue();
        senderMock.Verify(
            mock => mock.Send(
                new BackupZoneDataCommand(
                    referenceId
                ),
                CancellationToken.None
            )
        );
    }

    [Theory, AutoMoqData]
    public async Task ResultShouldBeAFailureWhenBackupTaskReturnsFailure(
        // Given
        string referenceId,
        string importArtifactUrl,
        [Frozen] Mock<ISender> senderMock,
        ImportZoneDataCommandHandler handler
    )
    {
        var expected = "ARTIFACT_MANAGEMENT_IMPORT_STEP_BACKUP_FAILED";

        senderMock.GivenValidSetup();
        senderMock.GivenDefaultRevertImportCommand();
        senderMock.GivenFailureBackgroundTaskResult();

        // When
        var actual = await handler.Handle(
            new ImportZoneDataCommand(
                referenceId,
                importArtifactUrl
            ),
            CancellationToken.None
        );

        // Then
        actual.Success.Should().BeFalse();
        actual.ErrorCode.Should().Be(expected);
    }

    [Theory, AutoMoqData]
    public async Task VerifyServerIsPausedWhenCommandIsHandled(
        // Given
        string importArtifactUrl,
        string referenceId,
        [Frozen] Mock<ISender> senderMock,
        ImportZoneDataCommandHandler handler
    )
    {
        senderMock.GivenValidSetup();

        // When
        var actual = await handler.Handle(
            new ImportZoneDataCommand(
                referenceId,
                importArtifactUrl
            ),
            CancellationToken.None
        );

        // Then
        actual.Success.Should().BeTrue();
        senderMock.Verify(
            mock => mock.Send(
                new PauseServerCommand(),
                CancellationToken.None
            )
        );
    }

    [Theory, AutoMoqData]
    public async Task ReturnFailureWhenServerWasNotSuccessfullyPaused(
        // Given
        string referenceId,
        string importArtifactUrl,
        [Frozen] Mock<ISender> senderMock,
        ImportZoneDataCommandHandler handler
    )
    {
        var expected = "ARTIFACT_MANAGEMENT_IMPORT_STEP_PAUSE_FAILED";

        senderMock.GivenValidSetup();
        senderMock.GivenDefaultRevertImportCommand();
        senderMock.GivenFailureOfPauseServerCommand();

        // When
        var actual = await handler.Handle(
            new ImportZoneDataCommand(
                referenceId,
                importArtifactUrl
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
    public async Task ReturnFailureWhenImportArtifactIsNotValid(
        // Given
        string referenceId,
        string importArtifactUrl,
        [Frozen] Mock<ISender> senderMock,
        ImportZoneDataCommandHandler handler
    )
    {
        var expected = "ARTIFACT_MANAGEMENT_IMPORT_STEP_DOMAIN_VALIDATION_FAILED";
        senderMock.GivenValidSetup();
        senderMock.GivenDefaultRevertImportCommand();

        senderMock.Setup(
            mock => mock.Send(
                new IsNotValidArtifactUrlDomain(
                    importArtifactUrl
                ),
                CancellationToken.None
            )
        ).ReturnsAsync(
            true
        );

        // When
        var actual = await handler.Handle(
            new ImportZoneDataCommand(
                referenceId,
                importArtifactUrl
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
        Uri importArtifactUri,
        [Frozen] Mock<ISender> senderMock,
        ImportZoneDataCommandHandler handler
    )
    {
        var expected = "ARTIFACT_MANAGEMENT_IMPORT_STEP_DOWNLOAD_ARTIFACT_FAILED";

        senderMock.GivenValidSetup();
        senderMock.GivenDefaultRevertImportCommand();

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
            new ImportZoneDataCommand(
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
    public async Task ReturnErrorWhenDeleteAppDataPathFails(
        // Given
        string referenceId,
        Uri importArtifactUri,
        [Frozen] Mock<ISender> senderMock,
        ImportZoneDataCommandHandler handler
    )
    {
        var expected = "ARTIFACT_MANAGEMENT_IMPORT_STEP_DELETE_DATA_FAILED";

        senderMock.GivenValidSetup();
        senderMock.GivenDefaultRevertImportCommand();

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
            new ImportZoneDataCommand(
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
        Uri importArtifactUri,
        [Frozen] Mock<ISender> senderMock,
        ImportZoneDataCommandHandler handler
    )
    {
        var expected = "ARTIFACT_MANAGEMENT_IMPORT_STEP_EXTRACT_FAILED";

        senderMock.GivenValidSetup();
        senderMock.GivenDefaultRevertImportCommand();

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
            new ImportZoneDataCommand(
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
    public async Task ReturnErrorWhenRestartServerFails(
        // Given
        string referenceId,
        Uri importArtifactUri,
        [Frozen] Mock<ISender> senderMock,
        ImportZoneDataCommandHandler handler
    )
    {
        var expected = "ARTIFACT_MANAGEMENT_IMPORT_STEP_RESTART_SERVER_FAILED";

        senderMock.GivenValidSetup();
        senderMock.GivenDefaultRevertImportCommand();

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
            new ImportZoneDataCommand(
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
    public async Task ReturnErrorWhenUnmanagedErrorIsThrown(
        // Given
        string referenceId,
        Uri importArtifactUri,
        [Frozen] Mock<ISender> senderMock,
        ImportZoneDataCommandHandler handler
    )
    {
        var expected = "ARTIFACT_MANAGEMENT_IMPORT_EXCEPTION";

        senderMock.GivenDefaultRevertImportCommand();

        senderMock.Setup(
            mock => mock.Send(
                It.IsAny<BackupZoneDataCommand>(),
                CancellationToken.None
            )
        ).ThrowsAsync(
            new Exception("I am unmanaged!")
        );

        // When
        var actual = await handler.Handle(
            new ImportZoneDataCommand(
                referenceId,
                importArtifactUri.ToString()
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
                It.IsAny<BackupZoneDataCommand>(),
                It.IsAny<CancellationToken>()
            )
        ).ReturnsAsync(
            new CommandResult<BackupZoneDataResult>(
                new BackupZoneDataResult(
                    "service",
                    "path"
                )
            )
        );

        senderMock.Setup(
            mock => mock.Send(
                It.IsAny<PauseServerCommand>(),
                It.IsAny<CancellationToken>()
            )
        ).ReturnsAsync(
            new StandardCommandResult()
        );

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

    public static void GivenDefaultRevertImportCommand(
        this Mock<ISender> senderMock
    )
    {
        senderMock.Setup(
            mock => mock.Send(
                It.IsAny<TriggerZoneArtifactRevertImportCommand>(),
                It.IsAny<CancellationToken>()
            )
        ).ReturnsAsync(
            new CommandResult<TriggerZoneArtifactRevertImportResult>(
                new TriggerZoneArtifactRevertImportResult(
                    string.Empty
                )
            )
        );
    }

    public static void GivenFailureBackgroundTaskResult(
        this Mock<ISender> senderMock
    )
    {
        senderMock.Setup(
            mock => mock.Send(
                It.IsAny<BackupZoneDataCommand>(),
                It.IsAny<CancellationToken>()
            )
        ).ReturnsAsync(
            new CommandResult<BackupZoneDataResult>(
                "error"
            )
        );
    }

    public static void GivenFailureOfPauseServerCommand(
        this Mock<ISender> senderMock,
        string errorCode = "error-code"
    )
    {
        senderMock.Setup(
            mock => mock.Send(
                It.IsAny<PauseServerCommand>(),
                It.IsAny<CancellationToken>()
            )
        ).ReturnsAsync(
            new StandardCommandResult(
                errorCode
            )
        );
    }
}
