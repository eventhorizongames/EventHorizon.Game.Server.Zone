namespace EventHorizon.Zone.System.ArtifactManagement.Trigger;

using EventHorizon.Zone.Core.Model.Command;

using MediatR;

public record TriggerZoneArtifactBackupCommand()
    : IRequest<CommandResult<TriggerZoneArtifactBackupResult>>;

public class TriggerZoneArtifactBackupResult
{
    public string ReferenceId { get; }

    public TriggerZoneArtifactBackupResult(
        string referenceId
    )
    {
        ReferenceId = referenceId;
    }
}

