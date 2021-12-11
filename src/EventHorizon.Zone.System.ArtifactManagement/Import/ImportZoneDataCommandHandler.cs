namespace EventHorizon.Zone.System.ArtifactManagement.Import;

using EventHorizon.Zone.Core.Events.DirectoryService;
using EventHorizon.Zone.Core.Events.FileService;
using EventHorizon.Zone.Core.Events.Lifetime;
using EventHorizon.Zone.Core.Model.Command;
using EventHorizon.Zone.Core.Model.Exceptions;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.System.Admin.Plugin.Command.Events;
using EventHorizon.Zone.System.Admin.Plugin.Command.Model.Builder;
using EventHorizon.Zone.System.ArtifactManagement.Backup;
using EventHorizon.Zone.System.ArtifactManagement.Query;
using EventHorizon.Zone.System.ArtifactManagement.Trigger;

using global::System;
using global::System.IO;
using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

using Microsoft.Extensions.Logging;

using static EventHorizon.Zone.System.ArtifactManagement.Model.ArtifactManagementSystemErrorCodes;

public class ImportZoneDataCommandHandler
    : IRequestHandler<ImportZoneDataCommand, StandardCommandResult>
{
    private readonly ILogger _logger;
    private readonly ISender _sender;
    private readonly IPublisher _publisher;
    private readonly ServerInfo _serverInfo;

    public ImportZoneDataCommandHandler(
        ILogger<ImportZoneDataCommandHandler> logger,
        ISender sender,
        IPublisher publisher,
        ServerInfo serverInfo
    )
    {
        _logger = logger;
        _sender = sender;
        _publisher = publisher;
        _serverInfo = serverInfo;
    }

    public async Task<StandardCommandResult> Handle(
        ImportZoneDataCommand request,
        CancellationToken cancellationToken
    )
    {
        var success = true;
        var backupUrl = string.Empty;
        try
        {
            var backupResult = await _sender.Send(
                new BackupZoneDataCommand(
                    request.ReferenceId
                ),
                cancellationToken
            );
            if (!backupResult)
            {
                throw LogAndCreateException(
                    backupResult,
                    ARTIFACT_MANAGEMENT_IMPORT_STEP_BACKUP_FAILED,
                    "Failed to Backup the currently existing Data."
                );
            }
            backupUrl = backupResult.Result.Path;

            var pauseServerResult = await _sender.Send(
                new PauseServerCommand(),
                cancellationToken
            );
            if (!pauseServerResult)
            {
                throw LogAndCreateException(
                    pauseServerResult,
                    ARTIFACT_MANAGEMENT_IMPORT_STEP_PAUSE_FAILED,
                    "Failed to Pause the Server."
                );
            }

            var isNotValidArtifactDomain = await _sender.Send(
                new IsNotValidArtifactUrlDomain(
                    request.ImportArtifactUrl
                ),
                cancellationToken
            );

            if (isNotValidArtifactDomain)
            {
                throw LogAndCreateException<bool>(
                    ARTIFACT_MANAGEMENT_IMPORT_STEP_DOMAIN_VALIDATION_FAILED,
                    ARTIFACT_MANAGEMENT_IMPORT_STEP_DOMAIN_VALIDATION_FAILED,
                    $"The Import Artifact URL domain of {request.ImportArtifactUrl} is not valid for Importing."
                );
            }

            var artifactDownloadedResult = await _sender.Send(
                new DownloadFileFromRemoteUrlCommand(
                    request.ImportArtifactUrl,
                    Path.Combine(
                        _serverInfo.TempPath,
                        "Import",
                        Path.GetFileName(
                            request.ImportArtifactUrl
                        )
                    )
                ),
                cancellationToken
            );
            if (!artifactDownloadedResult)
            {
                throw LogAndCreateException(
                    artifactDownloadedResult,
                    ARTIFACT_MANAGEMENT_IMPORT_STEP_DOWNLOAD_ARTIFACT_FAILED,
                    $"The Import Artifact of {request.ImportArtifactUrl} was not successfully downloaded."
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
                    ARTIFACT_MANAGEMENT_IMPORT_STEP_DELETE_DATA_FAILED,
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
                    ARTIFACT_MANAGEMENT_IMPORT_STEP_EXTRACT_FAILED,
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
            success = false;
            _logger.LogError(
                ex,
                "Failed on a Platform Exception."
            );

            return ex.ErrorCode;
        }
        catch (Exception ex)
        {
            success = false;
            _logger.LogError(
                ex,
                "Failed on a non-Platform Exception."
            );

            return ARTIFACT_MANAGEMENT_IMPORT_EXCEPTION;
        }
        finally
        {
            if (!success)
            {
                await _sender.Send(
                    new TriggerZoneArtifactRevertImportCommand(
                        backupUrl
                    ),
                    cancellationToken
                );
            }
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
