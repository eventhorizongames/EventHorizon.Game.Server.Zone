using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Editor.Assets.Scripts.Model;
using EventHorizon.Game.Server.Zone.External.Info;
using MediatR;
using IOPath = System.IO.Path;

namespace EventHorizon.Game.Server.Zone.Editor.Assets.Scripts
{
    public struct GetScriptFileContentHandler : IRequestHandler<GetScriptFileContentEvent, EditorScriptFileContent>
    {
        readonly ServerInfo _serverInfo;
        public GetScriptFileContentHandler(
            ServerInfo serverInfo
        )
        {
            _serverInfo = serverInfo;
        }
        public async Task<EditorScriptFileContent> Handle(GetScriptFileContentEvent request, CancellationToken cancellationToken)
        {
            return new EditorScriptFileContent
            {
                FileDirectory = request.Directory,
                FileName = request.FileName,
                Content = await File.ReadAllTextAsync(
                    IOPath.Combine(
                        _serverInfo.ScriptsPath,
                        request.Directory,
                        request.FileName
                    )
                )
            };
        }
    }
}