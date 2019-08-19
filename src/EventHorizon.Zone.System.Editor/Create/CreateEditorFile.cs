using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.External.Info;
using EventHorizon.Zone.System.Editor.Model;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventHorizon.Zone.System.Editor.Create
{
    public struct CreateEditorFile : IRequest<EditorResponse>
    {
        public IList<string> FilePath { get; }
        public string FileName { get; }

        public CreateEditorFile(
            IList<string> path,
            string fileName
        )
        {
            FilePath = path;
            FileName = fileName;
        }

        public struct CreateEditorFileHandler : IRequestHandler<CreateEditorFile, EditorResponse>
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

            public Task<EditorResponse> Handle(
                CreateEditorFile request,
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
                        return Task.FromResult(
                            new EditorResponse(
                                false,
                                "file_already_exists"
                            )
                        );
                    }
                    if (!fileInfo.Directory.Exists)
                    {
                        fileInfo.Directory.Create();
                    }
                    using (fileInfo.Create()) { }

                    return Task.FromResult(
                        new EditorResponse(
                            true
                        )
                    );
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        "Failed to Create Editor File.",
                        ex
                    );
                    return Task.FromResult(
                        new EditorResponse(
                            false,
                            "server_exception"
                        )
                    );
                }
            }
        }
    }
}