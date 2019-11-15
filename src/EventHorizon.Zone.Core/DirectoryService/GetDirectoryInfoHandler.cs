using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.DirectoryService;
using EventHorizon.Zone.Core.Model.DirectoryService;
using MediatR;

namespace EventHorizon.Zone.Core.DirectoryService
{
    public class GetDirectoryInfoHandler : IRequestHandler<GetDirectoryInfo, StandardDirectoryInfo>
    {
        readonly DirectoryResolver _directoryResolver;

        public GetDirectoryInfoHandler(
            DirectoryResolver directoryResolver
        )
        {
            _directoryResolver = directoryResolver;
        }
        
        public Task<StandardDirectoryInfo> Handle(
            GetDirectoryInfo request,
            CancellationToken cancellationToken
        ) => Task.FromResult(
            _directoryResolver.GetDirectoryInfo(
                request.DirectoryFullName
            )
        );
    }
}