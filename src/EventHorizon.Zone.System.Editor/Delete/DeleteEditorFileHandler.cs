namespace EventHorizon.Zone.System.Editor.Delete
{
    using EventHorizon.Zone.Core.Events.FileService;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.System.Backup.Events;
    using EventHorizon.Zone.System.Editor.Events.Delete;
    using EventHorizon.Zone.System.Editor.Model;

    using global::System;
    using global::System.IO;
    using global::System.Linq;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    using Microsoft.Extensions.Logging;

    public class DeleteEditorFileHandler : IRequestHandler<DeleteEditorFile, EditorResponse>
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly ServerInfo _serverInfo;

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
