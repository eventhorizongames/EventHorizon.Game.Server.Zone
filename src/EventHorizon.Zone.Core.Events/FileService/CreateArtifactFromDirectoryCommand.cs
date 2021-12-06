namespace EventHorizon.Zone.Core.Events.FileService;

using EventHorizon.Zone.Core.Model.Command;
using EventHorizon.Zone.Core.Model.DirectoryService;
using EventHorizon.Zone.Core.Model.FileService;

using MediatR;

public record CreateArtifactFromDirectoryCommand(
    StandardDirectoryInfo ArtifactSource,
    string ArtifactFileFullName
) : IRequest<CommandResult<StandardFileInfo>>;
