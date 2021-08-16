namespace EventHorizon.Zone.System.Editor.Create
{
    using EventHorizon.Zone.Core.Events.DirectoryService;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.System.Editor.Events.Create;
    using EventHorizon.Zone.System.Editor.Model;

    using global::System;
    using global::System.IO;
    using global::System.Linq;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    using Microsoft.Extensions.Logging;

    public class CreateEditorFolderHandler : IRequestHandler<CreateEditorFolder, EditorResponse>
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly ServerInfo _serverInfo;

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

        public async Task<EditorResponse> Handle(
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
                if (await _mediator.Send(
                    new DoesDirectoryExist(
                        folderPath
                    )
                ))
                {
                    _logger.LogError(
                        "Directory already exists. {FilePath} | {FolderName}",
                        request.FilePath,
                        request.FolderName
                    );
                    return new EditorResponse(
                        false,
                        "folder_already_exists"
                    );
                }
                if (!await _mediator.Send(
                    new CreateDirectory(
                        folderPath
                    )
                ))
                {
                    _logger.LogError(
                        "Directory failed to create. {FilePath} | {FolderName}",
                        request.FilePath,
                        request.FolderName
                    );
                    return new EditorResponse(
                        false,
                        "folder_failed_to_create"
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
                    "Failed to Create Editor Directory.",
                    request.FilePath,
                    request.FolderName
                );
                return new EditorResponse(
                    false,
                    "server_exception"
                );
            }
        }
    }
}
