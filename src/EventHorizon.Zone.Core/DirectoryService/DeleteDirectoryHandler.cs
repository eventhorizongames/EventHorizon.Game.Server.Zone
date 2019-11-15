using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.DirectoryService;
using EventHorizon.Zone.Core.Model.DirectoryService;
using MediatR;

namespace EventHorizon.Zone.Core.DirectoryService
{
    public class DeleteDirectoryHandler : IRequestHandler<DeleteDirectory, bool>
    {
        readonly DirectoryResolver _directoryResolver;

        public DeleteDirectoryHandler(
            DirectoryResolver directoryResolver
        )
        {
            _directoryResolver = directoryResolver;
        }

        public Task<bool> Handle(
            DeleteDirectory request,
            CancellationToken cancellationToken
        )
        {
            if (_directoryResolver.IsEmpty(
                request.DirectoryFullName
            ))
            {
                _directoryResolver.DeleteDirectory(
                    request.DirectoryFullName
                );
                return Task.FromResult(
                    true
                );
            }
            return Task.FromResult(
                false
            );
        }
    }
}