namespace EventHorizon.Zone.Core.Events.FileService;

using System.Collections.Generic;

using EventHorizon.Zone.Core.Model.Command;
using EventHorizon.Zone.Core.Model.DirectoryService;
using EventHorizon.Zone.Core.Model.FileService;

using MediatR;

public record CreateArtifactFromDirectoryCommand(
    StandardDirectoryInfo ArtifactSource,
    string ArtifactFileFullName,
    IEnumerable<string> Exclude
) : IRequest<CommandResult<StandardFileInfo>>;
