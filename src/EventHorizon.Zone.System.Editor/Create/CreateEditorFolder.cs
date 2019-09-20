using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.System.Editor.Model;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventHorizon.Zone.System.Editor.Create
{
    public struct CreateEditorFolder : IRequest<EditorResponse>
    {
        public IList<string> FilePath { get; }
        public string FolderName { get; }

        public CreateEditorFolder(
            IList<string> path,
            string folderName
        )
        {
            FilePath = path;
            FolderName = folderName;
        }

        public struct CreateEditorFolderHandler : IRequestHandler<CreateEditorFolder, EditorResponse>
        {
            readonly ILogger _logger;
            readonly IMediator _mediator;
            readonly ServerInfo _serverInfo;

            public CreateEditorFolderHandler(
                ILogger<CreateEditorFolderHandler> logger,
                IMediator mediator,
                ServerInfo serverInfo
            )
            {
                _logger = logger;
                _mediator = mediator;
                _serverInfo = serverInfo;
            }

            public Task<EditorResponse> Handle(
                CreateEditorFolder request, 
                CancellationToken cancellationToken
            )
            {
                try
                {
                    var folderPath = Path.Combine(
                        _serverInfo.AppDataPath,
                        Path.Combine(
                            request.FilePath.ToArray()
                        ),
                        request.FolderName
                    );
                    var folderInfo = new DirectoryInfo(
                        folderPath
                    );
                    if (folderInfo.Exists)
                    {
                        return Task.FromResult(
                            new EditorResponse(
                                false,
                                "folder_already_exists"
                            )
                        );
                    }
                    folderInfo.Create();

                    return Task.FromResult(
                        new EditorResponse(
                            true
                        )
                    );
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        "Failed to Create Editor Folder.",
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