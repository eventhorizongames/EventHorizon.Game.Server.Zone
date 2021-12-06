namespace EventHorizon.Zone.Core.DirectoryService;

using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;

using EventHorizon.Zone.Core.Events.DirectoryService;
using EventHorizon.Zone.Core.Model.Command;
using EventHorizon.Zone.Core.Model.DirectoryService;

using MediatR;

public class ExtractArtifactIntoDirectoryCommandHandler
    : IRequestHandler<ExtractArtifactIntoDirectoryCommand, CommandResult<StandardDirectoryInfo>>
{
    private readonly DirectoryResolver _resolver;

    public ExtractArtifactIntoDirectoryCommandHandler(
        DirectoryResolver resolver
    )
    {
        _resolver = resolver;
    }

    public Task<CommandResult<StandardDirectoryInfo>> Handle(
        ExtractArtifactIntoDirectoryCommand request,
        CancellationToken cancellationToken
    )
    {
        var fileLocation = request.FileInfo.FullName;
        var directoryFullName = request.DirectoryFullName;

        ZipFile.ExtractToDirectory(
            fileLocation,
            directoryFullName
        );

        return _resolver.GetDirectoryInfo(
            request.DirectoryFullName
        ).ToCommandResult()
        .FromResult();
    }
}
