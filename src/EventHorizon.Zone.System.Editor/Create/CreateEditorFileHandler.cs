namespace EventHorizon.Zone.System.Editor.Create;

using EventHorizon.Zone.Core.Events.FileService;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.System.Editor.Events.Create;
using EventHorizon.Zone.System.Editor.Model;

using global::System;
using global::System.IO;
using global::System.Linq;
using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

using Microsoft.Extensions.Logging;

public class CreateEditorFileHandler : IRequestHandler<CreateEditorFile, EditorResponse>
{
    private readonly ILogger _logger;
    private readonly IMediator _mediator;
    private readonly ServerInfo _serverInfo;

    public CreateEditorFileHandler(
        ILogger<CreateEditorFileHandler> logger,
        IMediator mediator,
        ServerInfo serverInfo
    )
    {
        _logger = logger;
        _mediator = mediator;
        _serverInfo = serverInfo;
    }

    public async Task<EditorResponse> Handle(
        CreateEditorFile request,
        CancellationToken cancellationToken
    )
    {
        try
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
                _logger.LogError(
                    "Editor File already exists. {FilePath} | {FileName}",
                    request.FilePath,
                    request.FileName
                );
                return new EditorResponse(
                    false,
                    "file_already_exists"
                );
            }
            await _mediator.Send(
                new CreateFile(
                    fileFullName
                )
            );

            return new EditorResponse(
                true
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to Create Editor File. {FilePath} | {FileName}",
                request.FilePath,
                request.FileName
            );
            return new EditorResponse(
                false,
                "server_exception"
            );
        }
    }
}
