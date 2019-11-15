using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.FileService;
using EventHorizon.Zone.Core.Model.FileService;
using MediatR;

namespace EventHorizon.Zone.Core.FileService
{
    public class GetFileInfoHandler : IRequestHandler<GetFileInfo, StandardFileInfo>
    {
        readonly FileResolver _fileResolver;

        public GetFileInfoHandler(FileResolver fileResolver)
        {
            _fileResolver = fileResolver;
        }

        public Task<StandardFileInfo> Handle(
            GetFileInfo request, 
            CancellationToken cancellationToken
        ) => Task.FromResult(
            _fileResolver.GetFileInfo(
                request.FileFullName
            )
        );
    }
}