namespace EventHorizon.Zone.System.Editor.State;

using EventHorizon.Zone.Core.Events.FileService;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.System.Editor.Events.State;
using EventHorizon.Zone.System.Editor.Model;

using global::System.IO;
using global::System.Linq;
using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

/// <summary>
/// This Handler will get the file content based on the path and filename from the App_Data directory 
/// </summary>
public class GetEditorFileContentHandler : IRequestHandler<GetEditorFileContent, StandardEditorFile>
{
    public static readonly string INVALID_FILE_IDENTIFIER = "__invalid__";

    private readonly IMediator _mediator;
    private readonly ServerInfo _serverInfo;

    public GetEditorFileContentHandler(
        IMediator mediator,
        ServerInfo serverInfo
    )
    {
        _mediator = mediator;
        _serverInfo = serverInfo;
    }

    public async Task<StandardEditorFile> Handle(
        GetEditorFileContent request,
        CancellationToken cancellationToken
    )
    {
        var fileFullName = Path.Combine(
            _serverInfo.AppDataPath,
            Path.Combine(
                request.FilePath.ToArray()
            ),
            request.FileName
        );
        if (await _mediator.Send(
            new DoesFileExist(
                fileFullName
            )
        ))
        {
            return new StandardEditorFile(
                request.FileName,
                request.FilePath,
                await _mediator.Send(
                    new ReadAllTextFromFile(
                        fileFullName
                    )
                )
            );
        }
        return new StandardEditorFile(
            INVALID_FILE_IDENTIFIER,
            new string[] { INVALID_FILE_IDENTIFIER },
            INVALID_FILE_IDENTIFIER
        );
    }
}
