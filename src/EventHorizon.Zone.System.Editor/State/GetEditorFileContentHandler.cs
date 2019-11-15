using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.FileService;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.System.Editor.Events.State;
using EventHorizon.Zone.System.Editor.Model;
using MediatR;

namespace EventHorizon.Zone.System.Editor.State
{
    /// <summary>
    /// This Handler will get the file content based on the path and filename from the App_Data directory 
    /// </summary>
    public class GetEditorFileContentHandler : IRequestHandler<GetEditorFileContent, StandardEditorFile>
    {
        readonly IMediator _mediator;
        readonly ServerInfo _serverInfo;

        public GetEditorFileContentHandler(
            IMediator mediator,
            ServerInfo serverInfo
        )
        {
            _mediator = mediator;
            _serverInfo = serverInfo;
        }

        public async Task<StandardEditorFile> Handle(
            GetEditorFileContent request,
            CancellationToken cancellationToken
        )
        {
            var fileFullName = Path.Combine(
                _serverInfo.AppDataPath,
                Path.Combine(
                    request.FilePath.ToArray()
                ),
                request.FileName
            );
            if (await _mediator.Send(
                new DoesFileExist(
                    fileFullName
                )
            ))
            {
                return new StandardEditorFile(
                    request.FileName,
                    request.FilePath,
                    await _mediator.Send(
                        new ReadAllTextFromFile(
                            fileFullName
                        )
                    )
                );
            }
            return new StandardEditorFile(
                "__invalid__",
                new string[] { "__invalid__" },
                "__invalid__"
            );
        }
    }
}