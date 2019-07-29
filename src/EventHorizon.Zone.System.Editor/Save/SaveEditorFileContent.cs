using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.External.Info;
using EventHorizon.Zone.System.Backup.Events;
using EventHorizon.Zone.System.Editor.Model;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventHorizon.Zone.System.Editor.Save
{
    public struct SaveEditorFileContent : IRequest<EditorFileSaveResponse>
    {
        public IList<string> FilePath { get; }
        public string FileName { get; }
        public string Content { get; }

        public SaveEditorFileContent(
            IList<string> path,
            string fileName,
            string content
        )
        {
            FilePath = path;
            FileName = fileName;
            Content = content;
        }

        public struct SaveEditorFileContentHandler : IRequestHandler<SaveEditorFileContent, EditorFileSaveResponse>
        {
            readonly ILogger _logger;
            readonly IMediator _mediator;
            readonly ServerInfo _serverInfo;

            public SaveEditorFileContentHandler(
                ILogger<SaveEditorFileContentHandler> logger,
                IMediator mediator,
                ServerInfo serverInfo
            )
            {
                _logger = logger;
                _mediator = mediator;
                _serverInfo = serverInfo;
            }

            public async Task<EditorFileSaveResponse> Handle(
                SaveEditorFileContent request,
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

                    File.WriteAllText(
                        fileInfo.FullName, 
                        request.Content
                    );
                    
                    return new EditorFileSaveResponse(
                        true
                    );
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        "Failed to Save Editor File Content.",
                        ex
                    );
                    return new EditorFileSaveResponse(
                        false,
                        "server_exception"
                    );
                }
            }
        }
    }
}