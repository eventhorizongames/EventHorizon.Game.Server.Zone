namespace EventHorizon.Zone.System.AssetServer.Export;

using EventHorizon.Zone.Core.Model.Command;

using global::System.IO;

using MediatR;

public record UploadAssetServerExportArtifactCommand(
    string Service,
    string FileFullName,
    Stream Content
) : IRequest<CommandResult<UploadAssetServerExportArtifactResult>>;
