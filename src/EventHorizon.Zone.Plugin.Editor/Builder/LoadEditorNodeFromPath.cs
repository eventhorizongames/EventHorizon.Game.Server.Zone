using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.External.Extensions;
using EventHorizon.Zone.System.Editor.Model;

namespace EventHorizon.Zone.Plugin.Editor.Builder
{
    public static class LoadEditorNodeFromPath
    {
        public static Task<IEditorNode> Create(
            string rootFolderName,
            string rootFolderPath,
            string directoryToLoadPath,
            string nodeType = "EDITOR_CONTENT"
        )
        {
            var directoryInfo = new DirectoryInfo(
                directoryToLoadPath
            );

            // Create Directory to Load
            var directoryNode = new StandardEditorNode(
                directoryInfo.Name,
                true,
                new List<string>() { rootFolderName },
                "FOLDER"
            );

            LoadFromDirectoryInfo(
                directoryNode,
                rootFolderPath,
                directoryInfo,
                nodeType
            );

            return Task.FromResult(
                directoryNode as IEditorNode
            );
        }

        private static void LoadFromDirectoryInfo(
            IEditorNode parentNode,
            string scriptsPath,
            DirectoryInfo directoryInfo,
            string nodeType
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
                        subDirectoryInfo.Parent.FullName
                    ).Split(
                        Path.DirectorySeparatorChar
                    ),
                    "FOLDER"
                );
                parentNode.Children.Add(
                    folderNode
                );

                // Load From Directory
                LoadFromDirectoryInfo(
                    folderNode,
                    scriptsPath,
                    subDirectoryInfo,
                    nodeType
                );
            }
            // Load script files into Repository
            LoadFileIntoRepository(
                parentNode,
                scriptsPath,
                directoryInfo,
                nodeType
            );
        }


        private static void LoadFileIntoRepository(
            IEditorNode parentNode,
            string scriptsPath,
            DirectoryInfo directoryInfo,
            string nodeType
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
                        nodeType
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
                case ".csx":
                    return "csharp";
                default:
                    return "plaintext";
            }
        }
    }
}