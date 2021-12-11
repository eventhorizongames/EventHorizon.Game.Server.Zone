namespace EventHorizon.Zone.System.ArtifactManagement.Trigger;

using EventHorizon.Zone.Core.Model.Command;

using MediatR;

public record TriggerZoneArtifactRevertImportCommand(
    string BackupArtifactUrl
) : IRequest<CommandResult<TriggerZoneArtifactRevertImportResult>>;

public class TriggerZoneArtifactRevertImportResult
{
    public string ReferenceId { get; }

    public TriggerZoneArtifactRevertImportResult(
        string referenceId
    )
    {
        ReferenceId = referenceId;
    }
}
