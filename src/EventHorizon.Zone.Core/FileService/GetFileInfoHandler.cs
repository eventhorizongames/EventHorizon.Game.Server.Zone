namespace EventHorizon.Zone.Core.FileService
{
    using EventHorizon.Zone.Core.Events.FileService;
    using EventHorizon.Zone.Core.Model.FileService;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetFileInfoHandler : IRequestHandler<GetFileInfo, StandardFileInfo>
    {
        readonly FileResolver _fileResolver;

        public GetFileInfoHandler(
            FileResolver fileResolver
        )
        {
            _fileResolver = fileResolver;
        }

        public Task<StandardFileInfo> Handle(
            GetFileInfo request,
            CancellationToken cancellationToken
        ) => _fileResolver.GetFileInfo(
            request.FileFullName
        ).FromResult();
    }
}