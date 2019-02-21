using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.External.Info;
using MediatR;
using IOPath = System.IO.Path;

namespace EventHorizon.Game.Server.Zone.Editor.Assets.Scripts
{
    public struct SaveScriptFileContentHandler : INotificationHandler<SaveScriptFileContentEvent>
    {
        readonly ServerInfo _serverInfo;
        public SaveScriptFileContentHandler(
            ServerInfo serverInfo
        )
        {
            _serverInfo = serverInfo;
        }
        public Task Handle(SaveScriptFileContentEvent notification, CancellationToken cancellationToken)
        {
            var scriptDirectory = notification.ScriptFileContent.FileDirectory;
            var scriptFileName = notification.ScriptFileContent.FileName;
            var scriptContents = notification.ScriptFileContent.Content;

            var pathToFile = IOPath.Combine(
                _serverInfo.ScriptsPath, scriptDirectory, scriptFileName
            );
            File.WriteAllText(
                pathToFile,
                scriptContents
            );

            return Task.CompletedTask;
        }
    }
}