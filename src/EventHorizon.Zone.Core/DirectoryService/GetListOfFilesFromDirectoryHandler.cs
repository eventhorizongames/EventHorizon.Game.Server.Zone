using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.DirectoryService;
using EventHorizon.Zone.Core.Model.DirectoryService;
using EventHorizon.Zone.Core.Model.FileService;
using MediatR;

namespace EventHorizon.Zone.Core.DirectoryService
{
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
        ) => Task.FromResult(
            _directoryResolver.GetFiles(
                request.DirectoryFullName
            )
        );
    }
}