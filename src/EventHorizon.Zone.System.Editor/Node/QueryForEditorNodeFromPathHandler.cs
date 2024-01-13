namespace EventHorizon.Zone.System.Editor.Node;

using EventHorizon.Zone.Core.Events.DirectoryService;
using EventHorizon.Zone.Core.Model.DirectoryService;
using EventHorizon.Zone.System.Editor.Events.Node;
using EventHorizon.Zone.System.Editor.Model;

using global::System.Collections.Generic;
using global::System.IO;
using global::System.Linq;
using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

public class QueryForEditorNodeFromPathHandler : IRequestHandler<QueryForEditorNodeFromPath, IEditorNode>
{
    public static readonly string DEFAULT_NODE_TYPE = "EDITOR_CONTENT";

    private readonly IMediator _mediator;

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
        if (string.IsNullOrWhiteSpace(nodeType))
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
                        fileInfo.Extension
                    )
                )
            );
        }
    }

    private static string ComputeLanguageFromName(
        string fileExtension
    )
    {
        return fileExtension switch
        {
            ".js" => "javascript",
            ".json" => "json",
            ".csx" => "csharp",
            ".cs" => "csharp",
            _ => "plaintext",
        };
    }
}
