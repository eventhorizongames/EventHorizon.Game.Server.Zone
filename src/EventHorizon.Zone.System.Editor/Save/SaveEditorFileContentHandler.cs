using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.DirectoryService;
using EventHorizon.Zone.Core.Events.FileService;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.System.Backup.Events;
using EventHorizon.Zone.System.Editor.Events.Save;
using EventHorizon.Zone.System.Editor.Model;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventHorizon.Zone.System.Editor.Save
{
    public class SaveEditorFileContentHandler : IRequestHandler<SaveEditorFileContent, EditorResponse>
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
                var fileFullName = Path.Combine(
                    _serverInfo.AppDataPath,
                    Path.Combine(
                        request.FilePath.ToArray()
                    ),
                    request.FileName
                );
                var fileInfo = await _mediator.Send(
                    new GetFileInfo(
                        fileFullName
                    )
                );
                if (await _mediator.Send(
                    new DoesFileExist(
                        fileInfo.FullName
                    )
                ))
                {
                    await _mediator.Send(
                        new CreateBackupOfFileContentCommand(
                            request.FilePath,
                            request.FileName,
                            await _mediator.Send(
                                new ReadAllTextFromFile(
                                    fileInfo.FullName
                                )
                            )
                        )
                    );
                }

                await _mediator.Send(
                    new WriteAllTextToFile(
                        fileInfo.FullName,
                        request.Content
                    )
                );

                return new EditorResponse(
                    true
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to Save Editor File Content. {FilePath} | {FileName}",
                    request.FilePath,
                    request.FileName
                );
                return new EditorResponse(
                    false,
                    "server_exception"
                );
            }
        }
    }
}