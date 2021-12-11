namespace EventHorizon.Zone.System.AssetServer.Backup;

using EventHorizon.Zone.Core.Model.Command;

using global::System.IO;

using MediatR;

public record UploadAssetServerBackupArtifactCommand(
    string Service,
    string FileFullName,
    Stream Content
) : IRequest<CommandResult<UploadAssetServerBackupArtifactResult>>;
