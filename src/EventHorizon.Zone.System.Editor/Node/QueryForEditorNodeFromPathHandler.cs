using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Editor.Events.Node;
using EventHorizon.Zone.System.Editor.Model;
using MediatR;
using System.IO;
using EventHorizon.Zone.Core.Events.DirectoryService;
using System.Collections.Generic;
using EventHorizon.Zone.Core.Model.DirectoryService;
using System.Linq;
using System;

namespace EventHorizon.Zone.System.Editor.Node
{
    public class QueryForEditorNodeFromPathHandler : IRequestHandler<QueryForEditorNodeFromPath, IEditorNode>
    {
        public const string DEFAULT_NODE_TYPE = "EDITOR_CONTENT";

        readonly IMediator _mediator;

        public QueryForEditorNodeFromPathHandler(
            IMediator mediator
        )
        {
            _mediator = mediator;
        }

        public Task<IEditorNode> Handle(
            QueryForEditorNodeFromPath request,
            CancellationToken cancellationToken
        ) => Create(
            request.NodePath,
            request.RootDirectoryFullName,
            request.DirectoryToLoadFullName,
            request.NodeType
        );

        private async Task<IEditorNode> Create(
            IList<string> nodePath,
            string rootDirectoryFullName,
            string directoryToLoadFullName,
            string nodeType
        )
        {
            if (nodeType == null)
            {
                nodeType = DEFAULT_NODE_TYPE;
            }
            var directoryInfo = await _mediator.Send(
                new GetDirectoryInfo(
                    directoryToLoadFullName
                )
            );

            // Create Directory to Load
            var editorNode = new StandardEditorNode(
                directoryInfo.Name,
                true,
                nodePath,
                "FOLDER"
            );

            if (await _mediator.Send(
                new DoesDirectoryExist(
                    directoryInfo.FullName
                )
            ))
            {
                await LoadFromDirectoryInfo(
                    editorNode,
                    nodePath,
                    rootDirectoryFullName,
                    directoryInfo,
                    nodeType
                );
            }

            return editorNode as IEditorNode;
        }

        private async Task LoadFromDirectoryInfo(
            IEditorNode parentNode,
            IList<string> rootPath,
            string rootDirectoryFullName,
            StandardDirectoryInfo directoryInfo,
            string nodeType
        )
        {
            // Load Scripts from Sub-Directories
            foreach (var subDirectoryInfo in await _mediator.Send(
                new GetListOfDirectoriesFromDirectory(
                    directoryInfo.FullName
                )
            ))
            {
                // Add Folder Node
                var folderNode = new StandardEditorNode(
                    subDirectoryInfo.Name,
                    true,
                    MergePaths(
                        rootPath,
                        rootDirectoryFullName.MakePathRelative(
                            subDirectoryInfo.ParentFullName
                        ).Split(
                            Path.DirectorySeparatorChar
                        )
                    ),
                    "FOLDER"
                );
                parentNode.Children.Add(
                    folderNode
                );

                // Load From Directory
                await LoadFromDirectoryInfo(
                    folderNode,
                    rootPath,
                    rootDirectoryFullName,
                    subDirectoryInfo,
                    nodeType
                );
            }
            // Load script files into Repository
            await LoadFileIntoRepository(
                parentNode,
                rootPath,
                rootDirectoryFullName,
                directoryInfo,
                nodeType
            );
        }

        private IList<string> MergePaths(
            IList<string> rootPath,
            string[] extraPath
        )
        {
            return new List<string>(
                rootPath
            ).Concat(
                new List<string>(
                    extraPath
                )
            ).ToList();
        }

        private async Task LoadFileIntoRepository(
            IEditorNode parentNode,
            IList<string> rootPath,
            string rootDirectoryFullName,
            StandardDirectoryInfo directoryInfo,
            string nodeType
        )
        {
            foreach (var fileInfo in await _mediator.Send(
                new GetListOfFilesFromDirectory(
                    directoryInfo.FullName
                )
            ))
            {
                var path = rootDirectoryFullName.MakePathRelative(
                    fileInfo.DirectoryName
                );
                // Create File Node
                parentNode.Children.Add(
                    new StandardEditorNode(
                        fileInfo.Name,
                        false,
                        MergePaths(
                            rootPath,
                            rootDirectoryFullName.MakePathRelative(
                                fileInfo.DirectoryName
                            ).Split(
                                Path.DirectorySeparatorChar
                            )
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