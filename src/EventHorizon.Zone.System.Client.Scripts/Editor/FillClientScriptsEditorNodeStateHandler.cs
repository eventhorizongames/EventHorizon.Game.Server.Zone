using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.External.Extensions;
using EventHorizon.Game.Server.Zone.External.Info;
using EventHorizon.Zone.System.Editor.Events;
using EventHorizon.Zone.System.Editor.Model;
using MediatR;

namespace EventHorizon.Zone.System.Client.Scripts.Editor
{
    public struct FillClientScriptsEditorNodeStateHandler : INotificationHandler<FillEditorNodeState>
    {
        readonly ServerInfo _serverInfo;

        public FillClientScriptsEditorNodeStateHandler(
            ServerInfo serverInfo
        )
        {
            _serverInfo = serverInfo;
        }

        public Task Handle(
            FillEditorNodeState notification,
            CancellationToken cancellationToken
        )
        {
            var scriptDirectoryInfo = new DirectoryInfo(
                GetClientScriptsPath()
            );
            var scriptNode = new StandardEditorNode(
                scriptDirectoryInfo.Name,
                true,
                new List<string>() { "Client", scriptDirectoryInfo.Name },
                "FOLDER"
            );


            this.LoadFromDirectoryInfo(
                scriptNode,
                _serverInfo.ClientPath,
                scriptDirectoryInfo
            );


            var clientNode = new StandardEditorNode(
                "Client",
                true,
                new List<string>() { "Client" },
                "FOLDER"
            );
            clientNode.Children.Add(
                scriptNode
            );
            notification.AddNode(
                clientNode
            );

            return Task.CompletedTask;
        }

        private void LoadFromDirectoryInfo(
            IEditorNode parentNode,
            string scriptsPath,
            DirectoryInfo directoryInfo
        )
        {
            // Load Scripts from Sub-Directories
            foreach (var subDirectoryInfo in directoryInfo.GetDirectories())
            {
                // Add Folder Node
                var folderNode = new StandardEditorNode(
                    subDirectoryInfo.Name,
                    true,
                    scriptsPath.MakePathRelative(
                        subDirectoryInfo.FullName
                    ).Split(
                        Path.DirectorySeparatorChar
                    ),
                    "FOLDER"
                );
                parentNode.Children.Add(
                    folderNode
                );

                // Load From Directory
                this.LoadFromDirectoryInfo(
                    folderNode,
                    scriptsPath,
                    subDirectoryInfo
                );
            }
            // Load script files into Repository
            this.LoadFileIntoRepository(
                parentNode,
                scriptsPath,
                directoryInfo
            );
        }


        private void LoadFileIntoRepository(
            IEditorNode parentNode,
            string scriptsPath,
            DirectoryInfo directoryInfo
        )
        {
            foreach (var fileInfo in directoryInfo.GetFiles())
            {
                // Create File Node
                parentNode.Children.Add(
                    new StandardEditorNode(
                        fileInfo.Name,
                        false,
                        scriptsPath.MakePathRelative(
                            fileInfo.DirectoryName
                        ).Split(
                            Path.DirectorySeparatorChar
                        ),
                        "EDITOR_CONTENT"
                    ).AddProperty(
                        "language",
                        ComputeLanguageFromName(
                            fileInfo.Name
                        )
                    )
                );
            }
        }

        private static string ComputeLanguageFromName(
            string fileName
        )
        {
            var fileInfo = new FileInfo(
                fileName
            );
            switch (fileInfo.Extension)
            {
                case ".js":
                    return "javascript";
                case ".json":
                    return "json";
                default:
                    return "plaintext";
            }
        }

        private string GetClientScriptsPath()
        {
            return _serverInfo.ClientScriptsPath;
        }
    }
}