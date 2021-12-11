namespace EventHorizon.Zone.System.AssetServer.Base;

using EventHorizon.Zone.Core.Model.Command;
using EventHorizon.Zone.System.AssetServer.Model;

using global::System.IO;

using MediatR;

public record UploadFileToAssetServerCommand(
    string Type,
    string Url,
    string FileFullName,
    string Service,
    Stream Content
) : IRequest<CommandResult<UploadAssetServerArtifactResult>>;
