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

namespace EventHorizon.Zone.System.Editor.Delete
{
    public struct DeleteEditorFolder : IRequest<EditorResponse>
    {
        public IList<string> FolderPath { get; }
        public string FolderName { get; }

        public DeleteEditorFolder(
            IList<string> path,
            string folderName
        )
        {
            FolderPath = path;
            FolderName = folderName;
        }
    }

    public struct DeleteEditorFolderHandler : IRequestHandler<DeleteEditorFolder, EditorResponse>
    {
        readonly ILogger _logger;
        readonly IMediator _mediator;
        readonly ServerInfo _serverInfo;

        public DeleteEditorFolderHandler(
            ILogger<DeleteEditorFolderHandler> logger,
            IMediator mediator,
            ServerInfo serverInfo
        )
        {
            _logger = logger;
            _mediator = mediator;
            _serverInfo = serverInfo;
        }

        public Task<EditorResponse> Handle(
            DeleteEditorFolder request,
            CancellationToken cancellationToken
        )
        {
            try
            {
                var folderPath = Path.Combine(
                    _serverInfo.AppDataPath,
                    Path.Combine(
                        request.FolderPath.ToArray()
                    ),
                    request.FolderName
                );
                var folderInfo = new DirectoryInfo(
                    folderPath
                );
                if (folderInfo.GetFiles().Length > 0
                    || folderInfo.GetDirectories().Length > 0)
                {
                    return Task.FromResult(
                        new EditorResponse(
                            false,
                            "folder_not_empty"
                        )
                    );
                }
                folderInfo.Delete();

                return Task.FromResult(
                    new EditorResponse(
                        true
                    )
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    "Failed to Delete Editor File.",
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