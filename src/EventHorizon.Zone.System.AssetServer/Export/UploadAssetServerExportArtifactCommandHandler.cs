namespace EventHorizon.Zone.System.AssetServer.Export;

using EventHorizon.Zone.Core.Model.Command;
using EventHorizon.Zone.System.AssetServer.Base;

using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

public class UploadAssetServerExportArtifactCommandHandler
    : IRequestHandler<UploadAssetServerExportArtifactCommand, CommandResult<UploadAssetServerExportArtifactResult>>
{
    private readonly ISender _sender;
    private readonly AssetServerSystemSettings _settings;

    public UploadAssetServerExportArtifactCommandHandler(
        ISender sender,
        AssetServerSystemSettings settings
    )
    {
        _sender = sender;
        _settings = settings;
    }

    public async Task<CommandResult<UploadAssetServerExportArtifactResult>> Handle(
        UploadAssetServerExportArtifactCommand request,
        CancellationToken cancellationToken
    )
    {
        var url = $"{_settings.Server}/api/Export/{request.Service}/Upload";

        var result = await _sender.Send(
            new UploadFileToAssetServerCommand(
                "Export",
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

        return new UploadAssetServerExportArtifactResult(
            uploadResponse.Service,
            uploadResponse.Path
        );
    }
}
