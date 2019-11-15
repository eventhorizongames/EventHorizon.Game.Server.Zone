using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.FileService;
using EventHorizon.Zone.Core.Model.FileService;
using MediatR;

namespace EventHorizon.Zone.Core.FileService
{
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
        ) => Task.FromResult(
            _fileResolver.AppendTextToFile(
                request.FileFullName,
                request.Text
            )
        );
    }
}