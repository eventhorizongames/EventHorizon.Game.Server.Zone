namespace EventHorizon.Zone.System.Editor.Delete
{
    using EventHorizon.Zone.Core.Events.DirectoryService;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.System.Editor.Events.Delete;
    using EventHorizon.Zone.System.Editor.Model;
    using global::System;
    using global::System.IO;
    using global::System.Linq;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public class DeleteEditorFolderHandler : IRequestHandler<DeleteEditorFolder, EditorResponse>
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly ServerInfo _serverInfo;

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
                string folderFullName = Path.Combine(
                    _serverInfo.AppDataPath,
                    Path.Combine(
                        request.FolderPath.ToArray()
                    ),
                    request.FolderName
                );
                if (!await _mediator.Send(
                    new IsDirectoryEmpty(
                        folderFullName
                    )
                ))
                {
                    return new EditorResponse(
                        false,
                        "folder_not_empty"
                    );
                }
                if (!await _mediator.Send(
                      new DeleteDirectory(
                          folderFullName
                      )
                  ))
                {
                    return new EditorResponse(
                        false,
                        "folder_failed_to_delete"
                    );
                }

                return new EditorResponse(
                    true
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to Delete Editor Folder."
                );
                return new EditorResponse(
                    false,
                    "server_exception"
                );
            }
        }
    }
}