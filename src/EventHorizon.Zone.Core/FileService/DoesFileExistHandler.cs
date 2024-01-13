namespace EventHorizon.Zone.Core.FileService;

using System.Threading;
using System.Threading.Tasks;

using EventHorizon.Zone.Core.Events.FileService;
using EventHorizon.Zone.Core.Model.FileService;

using MediatR;

public class DoesFileExistHandler : IRequestHandler<DoesFileExist, bool>
{
    readonly FileResolver _fileResolver;

    public DoesFileExistHandler(
        FileResolver fileResolver
    )
    {
        _fileResolver = fileResolver;
    }

    public Task<bool> Handle(
        DoesFileExist request,
        CancellationToken cancellationToken
    ) => _fileResolver.DoesFileExist(
        request.FileFullName
    ).FromResult();
}
