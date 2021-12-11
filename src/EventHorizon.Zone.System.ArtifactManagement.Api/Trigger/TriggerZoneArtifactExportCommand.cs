namespace EventHorizon.Zone.System.ArtifactManagement.Trigger;

using EventHorizon.Zone.Core.Model.Command;

using MediatR;

public record TriggerZoneArtifactExportCommand()
    : IRequest<CommandResult<TriggerZoneArtifactExportResult>>;

public class TriggerZoneArtifactExportResult
{
    public string ReferenceId { get; }

    public TriggerZoneArtifactExportResult(
        string referenceId
    )
    {
        ReferenceId = referenceId;
    }
}
