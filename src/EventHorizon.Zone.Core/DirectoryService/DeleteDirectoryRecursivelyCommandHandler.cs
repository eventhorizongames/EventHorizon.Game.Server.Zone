namespace EventHorizon.Zone.Core.DirectoryService;

using System.Threading;
using System.Threading.Tasks;

using EventHorizon.Zone.Core.Events.DirectoryService;
using EventHorizon.Zone.Core.Model.Command;
using EventHorizon.Zone.Core.Model.DirectoryService;

using MediatR;

public class DeleteDirectoryRecursivelyCommandHandler
    : IRequestHandler<DeleteDirectoryRecursivelyCommand, StandardCommandResult>
{
    private readonly DirectoryResolver _directoryResolver;

    public DeleteDirectoryRecursivelyCommandHandler(
        DirectoryResolver directoryResolver
    )
    {
        _directoryResolver = directoryResolver;
    }

    public Task<StandardCommandResult> Handle(
        DeleteDirectoryRecursivelyCommand request,
        CancellationToken cancellationToken
    )
    {
        _directoryResolver.DeleteDirectory(
            request.DirectoryFullName,
            true
        );

        return new StandardCommandResult()
            .FromResult();
    }
}
