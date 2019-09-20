using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.System.Backup.Events;
using EventHorizon.Zone.System.Editor.Model;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventHorizon.Zone.System.Editor.Save
{
    public struct SaveEditorFileContent : IRequest<EditorResponse>
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

        public struct SaveEditorFileContentHandler : IRequestHandler<SaveEditorFileContent, EditorResponse>
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

            public async Task<EditorResponse> Handle(
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

                    if (!fileInfo.Directory.Exists)
                    {
                        fileInfo.Directory.Create();
                    }

                    File.WriteAllText(
                        fileInfo.FullName,
                        request.Content
                    );

                    return new EditorResponse(
                        true
                    );
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        "Failed to Save Editor File Content.",
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
}