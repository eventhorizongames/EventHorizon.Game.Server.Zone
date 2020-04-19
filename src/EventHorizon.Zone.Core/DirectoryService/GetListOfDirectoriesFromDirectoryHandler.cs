namespace EventHorizon.Zone.Core.DirectoryService
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using EventHorizon.Zone.Core.Events.DirectoryService;
    using EventHorizon.Zone.Core.Model.DirectoryService;
    using MediatR;

    public class GetListOfDirectoriesFromDirectoryHandler : IRequestHandler<GetListOfDirectoriesFromDirectory, IEnumerable<StandardDirectoryInfo>>
    {
        private readonly DirectoryResolver _directoryResolver;

        public GetListOfDirectoriesFromDirectoryHandler(
            DirectoryResolver directoryResolver
        )
        {
            _directoryResolver = directoryResolver;
        }

        public Task<IEnumerable<StandardDirectoryInfo>> Handle(
            GetListOfDirectoriesFromDirectory request,
            CancellationToken cancellationToken
        ) => Task.FromResult(
            _directoryResolver.GetDirectories(
                request.DirectoryFullName
            )
        );
    }
}