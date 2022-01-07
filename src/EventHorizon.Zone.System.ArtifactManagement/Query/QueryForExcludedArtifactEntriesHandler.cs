namespace EventHorizon.Zone.System.ArtifactManagement.Query;

using EventHorizon.Zone.Core.Events.DirectoryService;

using global::System.Collections.Generic;
using global::System.IO;
using global::System.Linq;
using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

public class QueryForExcludedArtifactEntriesHandler
    : IRequestHandler<QueryForExcludedArtifactEntries, IEnumerable<string>>
{
    private readonly ISender _sender;

    public QueryForExcludedArtifactEntriesHandler(
        ISender sender
    )
    {
        _sender = sender;
    }

    public async Task<IEnumerable<string>> Handle(
        QueryForExcludedArtifactEntries request,
        CancellationToken cancellationToken
    )
    {
        var filesToExclude = await _sender.Send(
            new GetListOfFilesFromDirectory(
                request.DirectoryFullName
            ),
            cancellationToken
        );

        return filesToExclude.Select(
            a => request.DirectoryFullName.MakePathRelative(
                a.FullName
            )
        );
    }
}
