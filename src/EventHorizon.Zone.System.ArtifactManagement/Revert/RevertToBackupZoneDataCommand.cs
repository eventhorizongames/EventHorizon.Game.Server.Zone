namespace EventHorizon.Zone.System.ArtifactManagement.Revert;

using EventHorizon.Zone.Core.Model.Command;

using MediatR;

public record RevertToBackupZoneDataCommand(
    string ReferenceId,
    string BackupArtifactUrl
) : IRequest<StandardCommandResult>;
