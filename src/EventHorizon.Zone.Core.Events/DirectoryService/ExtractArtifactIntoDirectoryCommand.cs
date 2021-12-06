namespace EventHorizon.Zone.Core.Events.DirectoryService;

using EventHorizon.Zone.Core.Model.Command;
using EventHorizon.Zone.Core.Model.DirectoryService;
using EventHorizon.Zone.Core.Model.FileService;

using MediatR;

public record ExtractArtifactIntoDirectoryCommand(
    StandardFileInfo FileInfo,
    string DirectoryFullName
) : IRequest<CommandResult<StandardDirectoryInfo>>;
