namespace EventHorizon.Zone.Core.DirectoryService
{
    using EventHorizon.Zone.Core.Events.DirectoryService;
    using EventHorizon.Zone.Core.Model.DirectoryService;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

    public class DeleteDirectoryHandler : IRequestHandler<DeleteDirectory, bool>
    {
        private readonly DirectoryResolver _directoryResolver;

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
                return true.FromResult();
            }
            return false.FromResult();
        }
    }
}