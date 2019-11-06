using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
        readonly ServerInfo _serverInfo;
        public GetEditorFileContentHandler(
            ServerInfo serverInfo
        )
        {
            _serverInfo = serverInfo;
        }
        public Task<StandardEditorFile> Handle(
            GetEditorFileContent request,
            CancellationToken cancellationToken
        )
        {
            var filePath = Path.Combine(
                _serverInfo.AppDataPath,
                Path.Combine(
                    request.FilePath.ToArray()
                ),
                request.FileName
            );
            if (File.Exists(
                filePath
            ))
            {
                return Task.FromResult(
                    new StandardEditorFile(
                        request.FileName,
                        request.FilePath,
                        File.ReadAllText(
                            filePath
                        )
                    )
                );
            }
            return Task.FromResult(
                new StandardEditorFile(
                    "__invalid__",
                    new string[] { "__invalid__" },
                    "__invalid__"
                )
            );
        }
    }
}