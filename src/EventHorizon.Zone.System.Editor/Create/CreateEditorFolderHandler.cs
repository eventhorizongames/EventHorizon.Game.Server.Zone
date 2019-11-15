using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.DirectoryService;
using EventHorizon.Zone.Core.Events.FileService;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.System.Editor.Events.Create;
using EventHorizon.Zone.System.Editor.Model;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventHorizon.Zone.System.Editor.Create
{
    public class CreateEditorFolderHandler : IRequestHandler<CreateEditorFolder, EditorResponse>
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