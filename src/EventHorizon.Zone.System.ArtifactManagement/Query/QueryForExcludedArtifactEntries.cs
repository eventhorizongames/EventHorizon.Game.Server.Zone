namespace EventHorizon.Zone.System.ArtifactManagement.Query;

using global::System.Collections.Generic;

using MediatR;

public record QueryForExcludedArtifactEntries(
    string DirectoryFullName
) : IRequest<IEnumerable<string>>;
