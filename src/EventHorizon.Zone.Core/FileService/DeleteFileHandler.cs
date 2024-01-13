namespace EventHorizon.Zone.Core.FileService
{
    using System.Threading;
    using System.Threading.Tasks;
    using EventHorizon.Zone.Core.Events.FileService;
    using EventHorizon.Zone.Core.Model.FileService;
    using MediatR;

    public class DeleteFileHandler : IRequestHandler<DeleteFile>
    {
        readonly FileResolver _fileResolver;

        public DeleteFileHandler(FileResolver fileResolver)
        {
            _fileResolver = fileResolver;
        }

        public Task Handle(DeleteFile request, CancellationToken cancellationToken)
        {
            _fileResolver.DeleteFile(request.FileFullName);
            return Task.CompletedTask;
        }
    }
}
