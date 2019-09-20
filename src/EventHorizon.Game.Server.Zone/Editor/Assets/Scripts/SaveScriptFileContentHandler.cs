using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Model.Info;
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

            var directoriesToCombine = new List<string>();
            directoriesToCombine.Add(
                _serverInfo.AppDataPath
            );
            directoriesToCombine.AddRange(
                scriptDirectory
            );
            directoriesToCombine.Add(
                scriptFileName
            );
            var pathToFile = IOPath.Combine(
                directoriesToCombine.ToArray()
            );
            File.WriteAllText(
                pathToFile,
                scriptContents
            );

            return Task.CompletedTask;
        }
    }
}