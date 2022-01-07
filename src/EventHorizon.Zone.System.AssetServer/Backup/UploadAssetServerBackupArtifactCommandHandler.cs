namespace EventHorizon.Zone.System.AssetServer.Backup;

using EventHorizon.Zone.Core.Model.Command;
using EventHorizon.Zone.System.AssetServer.Base;

using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

public class UploadAssetServerBackupArtifactCommandHandler
    : IRequestHandler<UploadAssetServerBackupArtifactCommand, CommandResult<UploadAssetServerBackupArtifactResult>>
{
    private readonly ISender _sender;
    private readonly AssetServerSystemSettings _settings;

    public UploadAssetServerBackupArtifactCommandHandler(
        ISender sender,
        AssetServerSystemSettings settings
    )
    {
        _sender = sender;
        _settings = settings;
    }

    public async Task<CommandResult<UploadAssetServerBackupArtifactResult>> Handle(
        UploadAssetServerBackupArtifactCommand request,
        CancellationToken cancellationToken
    )
    {
        var url = $"{_settings.Server}/api/Backup/{request.Service}/Upload";

        var result = await _sender.Send(
            new UploadFileToAssetServerCommand(
                "Backup",
                url,
                request.FileFullName,
                request.Service,
                request.Content
            ),
            cancellationToken
        );
        if (!result)
        {
            return result.ErrorCode;
        }
        var uploadResponse = result.Result;

        return new UploadAssetServerBackupArtifactResult(
            uploadResponse.Service,
            $"{_settings.PublicServer}{uploadResponse.Path}"
        );
    }
}
