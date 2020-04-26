namespace EventHorizon.Zone.Core.DirectoryService
{
    using EventHorizon.Zone.Core.Events.DirectoryService;
    using EventHorizon.Zone.Core.Model.DirectoryService;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetDirectoryInfoHandler : IRequestHandler<GetDirectoryInfo, StandardDirectoryInfo>
    {
        private readonly DirectoryResolver _directoryResolver;

        public GetDirectoryInfoHandler(
            DirectoryResolver directoryResolver
        )
        {
            _directoryResolver = directoryResolver;
        }

        public Task<StandardDirectoryInfo> Handle(
            GetDirectoryInfo request,
            CancellationToken cancellationToken
        ) => _directoryResolver.GetDirectoryInfo(
            request.DirectoryFullName
        ).FromResult();
    }
}