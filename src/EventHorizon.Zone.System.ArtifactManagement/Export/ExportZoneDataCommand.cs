namespace EventHorizon.Zone.System.ArtifactManagement.Export;
using EventHorizon.Zone.Core.Model.Command;

using MediatR;

public record ExportZoneDataCommand(
    string ReferenceId
) : IRequest<CommandResult<ExportZoneDataResult>>;

public record ExportZoneDataResult(
    string Service,
    string Path
);
