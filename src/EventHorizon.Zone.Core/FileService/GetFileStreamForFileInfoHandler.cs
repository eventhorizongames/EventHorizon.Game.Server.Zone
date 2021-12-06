namespace EventHorizon.Zone.Core.FileService;

using System.IO;
using System.Threading;
using System.Threading.Tasks;

using EventHorizon.Zone.Core.Events.FileService;
using EventHorizon.Zone.Core.Model.FileService;

using MediatR;

public class GetFileStreamForFileInfoHandler
    : IRequestHandler<GetFileStreamForFileInfo, FileStream>
{
    private readonly FileResolver _resolver;

    public GetFileStreamForFileInfoHandler(
        FileResolver resolver
    )
    {
        _resolver = resolver;
    }

    public Task<FileStream> Handle(
        GetFileStreamForFileInfo request,
        CancellationToken cancellationToken
    )
    {
        return _resolver.GetFileAsStream(
            request.FileInfo.FullName
        ).FromResult();
    }
}
