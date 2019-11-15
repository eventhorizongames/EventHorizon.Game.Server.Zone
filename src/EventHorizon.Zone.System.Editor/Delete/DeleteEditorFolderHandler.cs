using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.DirectoryService;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.System.Editor.Events.Delete;
using EventHorizon.Zone.System.Editor.Model;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventHorizon.Zone.System.Editor.Delete
{
    public class DeleteEditorFolderHandler : IRequestHandler<DeleteEditorFolder, EditorResponse>
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

        public async Task<EditorResponse> Handle(
            DeleteEditorFolder request,
            CancellationToken cancellationToken
        )
        {
            try
            {
                if (!await _mediator.Send(
                    new DeleteDirectory(
                        Path.Combine(
                            _serverInfo.AppDataPath,
                            Path.Combine(
                                request.FolderPath.ToArray()
                            ),
                            request.FolderName
                        )
                    )
                ))
                {
                    return new EditorResponse(
                        false,
                        "folder_not_empty"
                    );
                }

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