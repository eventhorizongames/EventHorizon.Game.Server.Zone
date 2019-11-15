using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.FileService;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.System.Editor.Events.Create;
using EventHorizon.Zone.System.Editor.Model;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventHorizon.Zone.System.Editor.Create
{
    public class CreateEditorFileHandler : IRequestHandler<CreateEditorFile, EditorResponse>
    {
        readonly ILogger _logger;
        readonly IMediator _mediator;
        readonly ServerInfo _serverInfo;

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
}