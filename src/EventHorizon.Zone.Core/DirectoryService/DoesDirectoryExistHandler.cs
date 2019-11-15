using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.DirectoryService;
using EventHorizon.Zone.Core.Model.DirectoryService;
using MediatR;

namespace EventHorizon.Zone.Core.DirectoryService
{
    public class DoesDirectoryExistHandler : IRequestHandler<DoesDirectoryExist, bool>
    {
        readonly DirectoryResolver _directoryResolver;

        public DoesDirectoryExistHandler(
            DirectoryResolver directoryResolver
        )
        {
            _directoryResolver = directoryResolver;
        }

        public Task<bool> Handle(
            DoesDirectoryExist request, 
            CancellationToken cancellationToken
        ) => Task.FromResult(
            _directoryResolver.DoesDirectoryExist(
                request.DirectoryFullName
            )
        );
    }
}