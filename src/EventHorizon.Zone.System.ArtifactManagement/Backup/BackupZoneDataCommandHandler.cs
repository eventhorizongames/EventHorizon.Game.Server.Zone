namespace EventHorizon.Zone.System.ArtifactManagement.Backup;

using EventHorizon.Zone.Core.Events.DirectoryService;
using EventHorizon.Zone.Core.Events.FileService;
using EventHorizon.Zone.Core.Model.Command;
using EventHorizon.Zone.Core.Model.DateTimeService;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.System.AssetServer.Backup;

using global::System;
using global::System.IO;
using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

using Microsoft.Extensions.Logging;

using static EventHorizon.Zone.System.ArtifactManagement.Model.ArtifactManagementSystemErrorCodes;

public class BackupZoneDataCommandHandler
    : IRequestHandler<BackupZoneDataCommand, CommandResult<BackupZoneDataResult>>
{
    private readonly ILogger _logger;
    private readonly ISender _sender;
    private readonly ServerInfo _serverInfo;
    private readonly IDateTimeService _dateTimeService;

    public BackupZoneDataCommandHandler(
        ILogger<BackupZoneDataCommandHandler> logger,
        ISender sender,
        ServerInfo serverInfo,
        IDateTimeService dateTimeService
    )
    {
        _logger = logger;
        _sender = sender;
        _serverInfo = serverInfo;
        _dateTimeService = dateTimeService;
    }

    public async Task<CommandResult<BackupZoneDataResult>> Handle(
        BackupZoneDataCommand request,
        CancellationToken cancellationToken
    )
    {
        var service = "Zone".ToLowerInvariant();
        var artifactSourcePath = _serverInfo.AppDataPath;
        var destinationPathFullName = Path.Combine(
            _serverInfo.TempPath,
            "Backups",
            service
        );
        var backupFileName = $"backup.{_dateTimeService.Now.Ticks}.{request.ReferenceId}.zip";
        var artifactFileFullName = Path.Combine(
            destinationPathFullName,
            backupFileName
        );

        var createDirectoryResult = await _sender.Send(
            new CreateDirectory(
                destinationPathFullName
            ),
            cancellationToken
        );
        if (!createDirectoryResult)
        {
            return ARTIFACT_MANAGEMENT_BACKUP_STEP_CREATE_DESTINATION_FAILED;
        }

        var cleanupBackupsResult = await CleanupExistingBackups(
            destinationPathFullName,
            cancellationToken
        );
        if (!cleanupBackupsResult)
        {
            return cleanupBackupsResult.ErrorCode;
        }

        var createResult = await _sender.Send(
            new CreateArtifactFromDirectoryCommand(
                await _sender.Send(
                    new GetDirectoryInfo(
                        artifactSourcePath
                    ),
                    cancellationToken
                ),
                artifactFileFullName
            ),
            cancellationToken
        );
        if (!createResult)
        {
            return ARTIFACT_MANAGEMENT_BACKUP_STEP_CREATE_DIRECTORY_FAILED;
        }
        var destinationFileInfo = createResult.Result;

        var destinationFileContent = await _sender.Send(
            new GetStreamForFileInfo(
                destinationFileInfo
            ),
            cancellationToken
        );
        var result = await _sender.Send(
            new UploadAssetServerBackupArtifactCommand(
                service,
                backupFileName,
                destinationFileContent
            ),
            cancellationToken
        );

        if (!result)
        {
            return ARTIFACT_MANAGEMENT_BACKUP_ARTIFACT_UPLOAD_FAILED;
        }

        return new BackupZoneDataResult(
            result.Result.Service,
            result.Result.Path
        );
    }

    private async Task<StandardCommandResult> CleanupExistingBackups(
        string backupDestination,
        CancellationToken cancellationToken
    )
    {
        var filesToDelete = await _sender.Send(
            new GetListOfFilesFromDirectory(
                backupDestination
            ),
            cancellationToken
        );
        try
        {
            foreach (var file in filesToDelete)
            {
                await _sender.Send(
                    new DeleteFile(
                        file.FullName
                    ),
                    cancellationToken
                );
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to Delete Existing local Backups."
            );

            return ARTIFACT_MANAGEMENT_BACKUP_STEP_DELETE_EXISTING_BACKUPS_FAILED;
        }

        return new();
    }
}
