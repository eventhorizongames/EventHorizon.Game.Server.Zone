using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.External.Info;
using EventHorizon.Zone.System.Backup.Events;
using EventHorizon.Zone.System.Editor.Model;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventHorizon.Zone.System.Editor.Delete
{
    public class DeleteEditorFile : IRequest<EditorResponse>
    {
        public IList<string> FilePath { get; }
        public string FileName { get; }

        public DeleteEditorFile(
            IList<string> path,
            string fileName
        )
        {
            FilePath = path;
            FileName = fileName;
        }
    }

    public struct DeleteEditorFileHandler : IRequestHandler<DeleteEditorFile, EditorResponse>
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
                var filePath = Path.Combine(
                    _serverInfo.AppDataPath,
                    Path.Combine(
                        request.FilePath.ToArray()
                    ),
                    request.FileName
                );
                var fileInfo = new FileInfo(
                    filePath
                );
                if (fileInfo.Exists)
                {
                    await _mediator.Send(
                        new CreateBackupOfFileContentCommand(
                            request.FilePath,
                            request.FileName,
                            File.ReadAllText(
                                fileInfo.FullName
                            )
                        )
                    );
                }
                fileInfo.Delete();

                return new EditorResponse(
                    true
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    "Failed to Delete Editor File.",
                    ex
                );
                return new EditorResponse(
                    false,
                    "server_exception"
                );
            }
        }
    }
}