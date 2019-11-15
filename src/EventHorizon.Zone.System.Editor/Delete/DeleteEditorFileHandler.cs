using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.FileService;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.System.Backup.Events;
using EventHorizon.Zone.System.Editor.Events.Delete;
using EventHorizon.Zone.System.Editor.Model;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventHorizon.Zone.System.Editor.Delete
{
    public class DeleteEditorFileHandler : IRequestHandler<DeleteEditorFile, EditorResponse>
    {
        readonly ILogger _logger;
        readonly IMediator _mediator;
        readonly ServerInfo _serverInfo;

        public DeleteEditorFileHandler(
            ILogger<DeleteEditorFileHandler> logger,
            IMediator mediator,
            ServerInfo serverInfo
        )
        {
            _logger = logger;
            _mediator = mediator;
            _serverInfo = serverInfo;
        }

        public async Task<EditorResponse> Handle(
            DeleteEditorFile request,
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
                    await _mediator.Send(
                        new CreateBackupOfFileContentCommand(
                            request.FilePath,
                            request.FileName,
                            await _mediator.Send(
                                new ReadAllTextFromFile(
                                    fileFullName
                                )
                            )
                        )
                    );
                }
                await _mediator.Send(
                    new DeleteFile(
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
                    "Failed to Delete Editor File. {FilePath} | {FileName}",
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