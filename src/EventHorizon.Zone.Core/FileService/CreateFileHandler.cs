using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.FileService;
using EventHorizon.Zone.Core.Model.FileService;
using MediatR;

namespace EventHorizon.Zone.Core.FileService
{
    public class CreateFileHandler : IRequestHandler<CreateFile, bool>
    {
        readonly FileResolver _fileResolver;

        public CreateFileHandler(
            FileResolver fileResolver
        )
        {
            _fileResolver = fileResolver;
        }

        public Task<bool> Handle(
            CreateFile request, 
            CancellationToken cancellationToken
        ) => Task.FromResult(
            _fileResolver.CreateFile(
                request.FileFullName
            )
        );
    }
}