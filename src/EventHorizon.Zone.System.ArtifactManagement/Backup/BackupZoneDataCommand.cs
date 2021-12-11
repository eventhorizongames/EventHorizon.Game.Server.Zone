namespace EventHorizon.Zone.System.ArtifactManagement.Backup;

using EventHorizon.Zone.Core.Model.Command;

using MediatR;

public record BackupZoneDataCommand(
    string ReferenceId
) : IRequest<CommandResult<BackupZoneDataResult>>;

public record BackupZoneDataResult(
    string Service,
    string Path
);
