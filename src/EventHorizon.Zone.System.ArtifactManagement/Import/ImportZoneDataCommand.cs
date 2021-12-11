namespace EventHorizon.Zone.System.ArtifactManagement.Import;
using EventHorizon.Zone.Core.Model.Command;

using MediatR;

public record ImportZoneDataCommand(
    string ReferenceId,
    string ImportArtifactUrl
) : IRequest<StandardCommandResult>;
