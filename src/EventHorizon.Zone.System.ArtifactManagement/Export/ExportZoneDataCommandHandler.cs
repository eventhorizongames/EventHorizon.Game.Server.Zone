namespace EventHorizon.Zone.System.ArtifactManagement.Export;

using EventHorizon.Zone.Core.Events.DirectoryService;
using EventHorizon.Zone.Core.Events.FileService;
using EventHorizon.Zone.Core.Model.Command;
using EventHorizon.Zone.Core.Model.DateTimeService;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.System.ArtifactManagement.Backup;
using EventHorizon.Zone.System.AssetServer.Export;

using global::System;
using global::System.IO;
using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

using Microsoft.Extensions.Logging;

using static EventHorizon.Zone.System.ArtifactManagement.Model.ArtifactManagementSystemErrorCodes;

public class ExportZoneDataCommandHandler
    : IRequestHandler<ExportZoneDataCommand, CommandResult<ExportZoneDataResult>>
{
    private readonly ILogger _logger;
    private readonly ISender _sender;
    private readonly ServerInfo _serverInfo;
    private readonly IDateTimeService _dateTimeService;

    public ExportZoneDataCommandHandler(
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

    public async Task<CommandResult<ExportZoneDataResult>> Handle(
        ExportZoneDataCommand request,
        CancellationToken cancellationToken
    )
    {

        var service = "Zone".ToLowerInvariant();
        var artifactSourcePath = _serverInfo.AppDataPath;
        var destinationPathFullName = Path.Combine(
            _serverInfo.TempPath,
            "Exports",
            service
        );
        var exportFileName = $"export.{_dateTimeService.Now.Ticks}.{request.ReferenceId}.zip";
        var artifactFileFullName = Path.Combine(
            destinationPathFullName,
            exportFileName
        );

        var createDirectoryResult = await _sender.Send(
            new CreateDirectory(
                destinationPathFullName
            ),
            cancellationToken
        );
        if (!createDirectoryResult)
        {
            return ARTIFACT_MANAGEMENT_EXPORT_STEP_CREATE_DESTINATION_FAILED;
        }

        var cleanupExportsResult = await CleanupExistingExports(
            destinationPathFullName,
            cancellationToken
        );
        if (!cleanupExportsResult)
        {
            return cleanupExportsResult.ErrorCode;
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
            return ARTIFACT_MANAGEMENT_EXPORT_STEP_CREATE_DIRECTORY_FAILED;
        }
        var destiniationFileInfo = createResult.Result;

        var destinationFileContent = await _sender.Send(
            new GetStreamForFileInfo(
                destiniationFileInfo
            ),
            cancellationToken
        );
        var result = await _sender.Send(
            new UploadAssetServerExportArtifactCommand(
                service,
                exportFileName,
                destinationFileContent 
            ),
            cancellationToken
        );

        if (!result)
        {
            return ARTIFACT_MANAGEMENT_EXPORT_ARTIFACT_UPLOAD_FAILED;
        }

        return new ExportZoneDataResult(
            result.Result.Service,
            result.Result.Url
        );
    }

    private async Task<StandardCommandResult> CleanupExistingExports(
        string exportDestination,
        CancellationToken cancellationToken
    )
    {
        var filesToDelete = await _sender.Send(
            new GetListOfFilesFromDirectory(
                exportDestination
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
                "Failed to Delete Existing local Exports"
            );

            return ARTIFACT_MANAGEMENT_EXPORT_STEP_DELETE_EXISTING_EXPORTS_FAILED;
        }

        return new();
    }
}
