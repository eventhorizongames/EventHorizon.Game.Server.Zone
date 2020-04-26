namespace EventHorizon.Zone.Core.DirectoryService
{
    using EventHorizon.Zone.Core.Events.DirectoryService;
    using EventHorizon.Zone.Core.Model.DirectoryService;
    using EventHorizon.Zone.Core.Model.FileService;
    using MediatR;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetListOfFilesFromDirectoryHandler : IRequestHandler<GetListOfFilesFromDirectory, IEnumerable<StandardFileInfo>>
    {
        readonly DirectoryResolver _directoryResolver;

        public GetListOfFilesFromDirectoryHandler(
            DirectoryResolver directoryResolver
        )
        {
            _directoryResolver = directoryResolver;
        }

        public Task<IEnumerable<StandardFileInfo>> Handle(
            GetListOfFilesFromDirectory request,
            CancellationToken cancellationToken
        ) => _directoryResolver.GetFiles(
            request.DirectoryFullName
        ).FromResult();
    }
}