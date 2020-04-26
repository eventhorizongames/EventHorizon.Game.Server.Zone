namespace EventHorizon.Zone.Core.FileService
{
    using EventHorizon.Zone.Core.Events.FileService;
    using EventHorizon.Zone.Core.Model.FileService;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

    public class ReadAllTextFromFileHandler : IRequestHandler<ReadAllTextFromFile, string>
    {
        readonly FileResolver _fileResolver;

        public ReadAllTextFromFileHandler(
            FileResolver fileResolver
        )
        {
            _fileResolver = fileResolver;
        }

        public Task<string> Handle(
            ReadAllTextFromFile request,
            CancellationToken cancellationToken
        ) => _fileResolver.GetFileText(
            request.FileFullName
        ).FromResult();
    }
}