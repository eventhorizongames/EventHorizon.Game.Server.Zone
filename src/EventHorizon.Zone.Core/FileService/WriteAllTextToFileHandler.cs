namespace EventHorizon.Zone.Core.FileService
{
    using EventHorizon.Zone.Core.Events.FileService;
    using EventHorizon.Zone.Core.Model.FileService;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

    public class WriteAllTextToFileHandler : IRequestHandler<WriteAllTextToFile>
    {
        readonly FileResolver _fileResolver;

        public WriteAllTextToFileHandler(
            FileResolver fileResolver
        )
        {
            _fileResolver = fileResolver;
        }

        public Task<Unit> Handle(
            WriteAllTextToFile request,
            CancellationToken cancellationToken
        )
        {
            _fileResolver.WriteAllText(
                request.FileFullName,
                request.Text
            );
            return Unit.Task;
        }
    }
}