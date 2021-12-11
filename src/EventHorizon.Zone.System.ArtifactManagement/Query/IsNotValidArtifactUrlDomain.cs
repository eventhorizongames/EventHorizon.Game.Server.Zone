namespace EventHorizon.Zone.System.ArtifactManagement.Query;

using MediatR;

public record IsNotValidArtifactUrlDomain(
    string ArtifactUrl
) : IRequest<bool>;
