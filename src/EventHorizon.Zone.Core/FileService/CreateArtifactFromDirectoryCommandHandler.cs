namespace EventHorizon.Zone.Core.FileService;

using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using EventHorizon.Zone.Core.Events.FileService;
using EventHorizon.Zone.Core.Model.Command;
using EventHorizon.Zone.Core.Model.FileService;

using MediatR;

public class CreateArtifactFromDirectoryCommandHandler
    : IRequestHandler<CreateArtifactFromDirectoryCommand, CommandResult<StandardFileInfo>>
{
    private readonly FileResolver _resolver;

    public CreateArtifactFromDirectoryCommandHandler(
        FileResolver resolver
    )
    {
        _resolver = resolver;
    }

    public Task<CommandResult<StandardFileInfo>> Handle(
        CreateArtifactFromDirectoryCommand request,
        CancellationToken cancellationToken
    )
    {
        ZipFile.CreateFromDirectory(
            request.ArtifactSource.FullName,
            request.ArtifactFileFullName
        );
        RemoveExcludedEntries(
            request
        );

        return _resolver.GetFileInfo(
            request.ArtifactFileFullName
        ).ToCommandResult()
        .FromResult();
    }

    private static void RemoveExcludedEntries(
        CreateArtifactFromDirectoryCommand request
    )
    {
        if (!request.Exclude.Any())
        {
            return;
        }

        using var archive = ZipFile.Open(
            request.ArtifactFileFullName,
            ZipArchiveMode.Update
        );
        var toRemoveEntries = new List<ZipArchiveEntry>();
        foreach (var entry in archive.Entries)
        {
            if (request.Exclude.Contains(
                entry.FullName
            ))
            {
                toRemoveEntries.Add(entry);
            }
        }

        foreach (var entry in toRemoveEntries)
        {
            entry.Delete();
        }
    }
}
