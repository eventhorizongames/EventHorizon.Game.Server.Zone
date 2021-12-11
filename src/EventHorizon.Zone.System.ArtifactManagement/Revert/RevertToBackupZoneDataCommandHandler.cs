namespace EventHorizon.Zone.System.ArtifactManagement.Revert;

using EventHorizon.Zone.Core.Events.DirectoryService;
using EventHorizon.Zone.Core.Events.FileService;
using EventHorizon.Zone.Core.Events.Lifetime;
using EventHorizon.Zone.Core.Model.Command;
using EventHorizon.Zone.Core.Model.Exceptions;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.System.Admin.Plugin.Command.Events;
using EventHorizon.Zone.System.Admin.Plugin.Command.Model.Builder;
using EventHorizon.Zone.System.ArtifactManagement.Query;

using global::System;
using global::System.IO;
using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

using Microsoft.Extensions.Logging;

using static EventHorizon.Zone.System.ArtifactManagement.Model.ArtifactManagementSystemErrorCodes;

public class RevertToBackupZoneDataCommandHandler
    : IRequestHandler<RevertToBackupZoneDataCommand, StandardCommandResult>
{
    private readonly ILogger _logger;
    private readonly ISender _sender;
    private readonly IPublisher _publisher;
    private readonly ServerInfo _serverInfo;
    private readonly ArtifactManagementSystemSettings _settings;

    public RevertToBackupZoneDataCommandHandler(
        ILogger<RevertToBackupZoneDataCommandHandler> logger,
        ISender sender,
        IPublisher publisher,
        ServerInfo serverInfo,
        ArtifactManagementSystemSettings settings
    )
    {
        _logger = logger;
        _sender = sender;
        _publisher = publisher;
        _serverInfo = serverInfo;
        _settings = settings;
    }

    public async Task<StandardCommandResult> Handle(
        RevertToBackupZoneDataCommand request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var notValidArtifactDomain = await _sender.Send(
                new IsNotValidArtifactUrlDomain(
                    request.BackupArtifactUrl
                ),
                cancellationToken
            );
            if (notValidArtifactDomain)
            {
                throw LogAndCreateException<bool>(
                    ARTIFACT_MANAGEMENT_REVERT_TO_BACKUP_STEP_DOMAIN_VALIDATION_FAILED,
                    ARTIFACT_MANAGEMENT_REVERT_TO_BACKUP_STEP_DOMAIN_VALIDATION_FAILED,
                    $"The Import Artifact URL domain of {request.BackupArtifactUrl} is not valid for Importing."
                );
            }

            var artifactDownloadedResult = await _sender.Send(
                new DownloadFileFromRemoteUrlCommand(
                    request.BackupArtifactUrl,
                    Path.Combine(
                        _serverInfo.TempPath,
                        "Backup",
                        Path.GetFileName(
                            request.BackupArtifactUrl
                        )
                    )
                ),
                cancellationToken
            );
            if (!artifactDownloadedResult)
            {
                throw LogAndCreateException(
                    artifactDownloadedResult,
                    ARTIFACT_MANAGEMENT_REVERT_TO_BACKUP_STEP_DOWNLOAD_ARTIFACT_FAILED,
                    $"The Backup Artifact of {request.BackupArtifactUrl} was not successfully downloaded."
                );
            }
            var artifactFileInfo = artifactDownloadedResult.Result.FileInfo;

            var deleteResult = await _sender.Send(
                new DeleteDirectoryRecursivelyCommand(
                    _serverInfo.AppDataPath
                ),
                cancellationToken
            );
            if (!deleteResult)
            {
                throw LogAndCreateException(
                    deleteResult,
                    ARTIFACT_MANAGEMENT_REVERT_TO_BACKUP_STEP_DELETE_DATA_FAILED,
                    $"Failed to delete {_serverInfo.AppDataPath}."
                );
            }
            var extractDirectoryResult = await _sender.Send(
                new ExtractArtifactIntoDirectoryCommand(
                    artifactFileInfo,
                    _serverInfo.AppDataPath
                ),
                cancellationToken
            );
            if (!extractDirectoryResult)
            {
                throw LogAndCreateException(
                    extractDirectoryResult,
                    ARTIFACT_MANAGEMENT_REVERT_TO_BACKUP_STEP_EXTRACT_FAILED,
                    $"Failed to extract {artifactFileInfo.FullName} into {_serverInfo.AppDataPath}."
                );
            }

            await _sender.Send(
                new RunServerStartupCommand(),
                cancellationToken
            );
            await _publisher.Publish(
                new AdminCommandEvent(
                    BuildAdminCommand.FromString(
                        "reload-system"
                    ),
                    "reload-system"
                ),
                cancellationToken
            );
            await _sender.Send(
                new FinishServerStartCommand(),
                cancellationToken
            );
        }
        catch (PlatformErrorCodeException ex)
        {
            _logger.LogError(
                ex,
                "Failed on a Platform Exception."
            );

            return ex.ErrorCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed on a non-Platform Exception."
            );

            return ARTIFACT_MANAGEMENT_REVERT_TO_BACKUP_EXCEPTION;
        }

        return new();
    }

    private Exception LogAndCreateException<TResult>(
        CommandResult<TResult> result,
        string platformErrorCode,
        string message
    )
    {
        _logger.LogError(
            "{Message} | ErrorCode: [{ErrorCode}]",
            message,
            result.ErrorCode
        );

        throw new PlatformErrorCodeException(
            platformErrorCode,
            message
        );
    }
}
