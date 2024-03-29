namespace EventHorizon.Zone.Core.DirectoryService;

using System.Threading;
using System.Threading.Tasks;

using EventHorizon.Zone.Core.Events.DirectoryService;
using EventHorizon.Zone.Core.Model.DirectoryService;

using MediatR;

public class DoesDirectoryExistHandler : IRequestHandler<DoesDirectoryExist, bool>
{
    private readonly DirectoryResolver _directoryResolver;

    public DoesDirectoryExistHandler(
        DirectoryResolver directoryResolver
    )
    {
        _directoryResolver = directoryResolver;
    }

    public Task<bool> Handle(
        DoesDirectoryExist request,
        CancellationToken cancellationToken
    ) => _directoryResolver.DoesDirectoryExist(
        request.DirectoryFullName
    ).FromResult();
}
