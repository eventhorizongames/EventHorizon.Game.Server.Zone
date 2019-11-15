using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.FileService;
using EventHorizon.Zone.Core.Model.FileService;
using MediatR;

namespace EventHorizon.Zone.Core.FileService
{
    public class DoesFileExistHandler : IRequestHandler<DoesFileExist, bool>
    {
        readonly FileResolver _fileResolver;

        public DoesFileExistHandler(
            FileResolver fileResolver
        )
        {
            _fileResolver = fileResolver;
        }

        public Task<bool> Handle(
            DoesFileExist request,
            CancellationToken cancellationToken
        ) => Task.FromResult(
            _fileResolver.DoesFileExist(
                request.FileFullName
            )
        );
    }
}