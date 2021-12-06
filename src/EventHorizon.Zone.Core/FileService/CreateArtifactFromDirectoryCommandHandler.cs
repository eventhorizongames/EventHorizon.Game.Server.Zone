namespace EventHorizon.Zone.Core.FileService;

using System.IO.Compression;
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

        return _resolver.GetFileInfo(
            request.ArtifactFileFullName
        ).ToCommandResult()
        .FromResult();
    }
}
