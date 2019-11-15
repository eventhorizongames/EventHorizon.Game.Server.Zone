using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.DirectoryService;
using EventHorizon.Zone.Core.Model.DirectoryService;
using MediatR;

namespace EventHorizon.Zone.Core.DirectoryService
{
    public class CreateDirectoryHandler : IRequestHandler<CreateDirectory, bool>
    {
        readonly DirectoryResolver _directoryResolver;

        public CreateDirectoryHandler(
            DirectoryResolver directoryResolver
        )
        {
            _directoryResolver = directoryResolver;
        }

        public Task<bool> Handle(
            CreateDirectory request, 
            CancellationToken cancellationToken
        ) => Task.FromResult(
            _directoryResolver.CreateDirectory(
                request.DirectoryFullName
            )
        );
    }
}