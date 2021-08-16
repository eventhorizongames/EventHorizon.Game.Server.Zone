namespace EventHorizon.Zone.Core.FileService
{
    using System.Threading;
    using System.Threading.Tasks;

    using EventHorizon.Zone.Core.Events.FileService;
    using EventHorizon.Zone.Core.Model.FileService;

    using MediatR;

    public class ReadAllTextAsBytesFromFileHandler
        : IRequestHandler<ReadAllTextAsBytesFromFile, byte[]>
    {
        readonly FileResolver _fileResolver;

        public ReadAllTextAsBytesFromFileHandler(
            FileResolver fileResolver
        )
        {
            _fileResolver = fileResolver;
        }

        public Task<byte[]> Handle(
            ReadAllTextAsBytesFromFile request,
            CancellationToken cancellationToken
        ) => _fileResolver.GetFileTextAsBytes(
            request.FileFullName
        ).FromResult();
    }
}
