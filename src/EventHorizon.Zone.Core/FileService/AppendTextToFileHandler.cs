namespace EventHorizon.Zone.Core.FileService
{
    using System.Threading;
    using System.Threading.Tasks;

    using EventHorizon.Zone.Core.Events.FileService;
    using EventHorizon.Zone.Core.Model.FileService;

    using MediatR;

    public class AppendTextToFileHandler : IRequestHandler<AppendTextToFile, bool>
    {
        readonly FileResolver _fileResolver;

        public AppendTextToFileHandler(
            FileResolver fileResolver
        )
        {
            _fileResolver = fileResolver;
        }

        public Task<bool> Handle(
            AppendTextToFile request,
            CancellationToken cancellationToken
        ) => _fileResolver.AppendTextToFile(
            request.FileFullName,
            request.Text
        ).FromResult();
    }
}
