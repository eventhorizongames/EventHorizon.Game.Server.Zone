namespace EventHorizon.Zone.Core.FileService;

using System.IO;
using System.Threading;
using System.Threading.Tasks;

using EventHorizon.Zone.Core.Events.FileService;
using EventHorizon.Zone.Core.Model.FileService;

using MediatR;

public class GetStreamForFileInfoHandler
    : IRequestHandler<GetStreamForFileInfo, Stream>
{
    private readonly FileResolver _resolver;

    public GetStreamForFileInfoHandler(
        FileResolver resolver
    )
    {
        _resolver = resolver;
    }

    public Task<Stream> Handle(
        GetStreamForFileInfo request,
        CancellationToken cancellationToken
    )
    {
        return _resolver.GetFileAsStream(
            request.FileInfo.FullName
        ).FromResult<Stream>();
    }
}
