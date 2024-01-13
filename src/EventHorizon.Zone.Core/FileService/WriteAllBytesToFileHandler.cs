namespace EventHorizon.Zone.Core.FileService;

using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.FileService;
using EventHorizon.Zone.Core.Model.FileService;
using MediatR;

public class WriteAllBytesToFileHandler : IRequestHandler<WriteAllBytesToFile>
{
    readonly FileResolver _fileResolver;

    public WriteAllBytesToFileHandler(FileResolver fileResolver)
    {
        _fileResolver = fileResolver;
    }

    public Task Handle(WriteAllBytesToFile request, CancellationToken cancellationToken)
    {
        _fileResolver.WriteAllBytes(request.FileFullName, request.Bytes);

        return Task.CompletedTask;
    }
}
