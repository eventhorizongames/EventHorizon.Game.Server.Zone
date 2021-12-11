namespace EventHorizon.Zone.System.ArtifactManagement.Trigger;

using EventHorizon.Zone.Core.Model.Command;

using MediatR;

public record TriggerZoneArtifactImportCommand(
    string ImportArtifactUrl
) : IRequest<CommandResult<TriggerZoneArtifactImportResult>>;

public class TriggerZoneArtifactImportResult
{
    public string ReferenceId { get; }

    public TriggerZoneArtifactImportResult(
        string referenceId
    )
    {
        ReferenceId = referenceId;
    }
}
