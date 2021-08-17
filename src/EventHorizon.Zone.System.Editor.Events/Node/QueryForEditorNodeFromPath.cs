namespace EventHorizon.Zone.System.Editor.Events.Node
{
    using EventHorizon.Zone.System.Editor.Model;

    using global::System.Collections.Generic;

    using MediatR;

    public struct QueryForEditorNodeFromPath
        : IRequest<IEditorNode>
    {
        public IList<string> NodePath { get; }
        public string RootDirectoryFullName { get; }
        public string DirectoryToLoadFullName { get; }
        public string NodeType { get; }

        public QueryForEditorNodeFromPath(
            IList<string> nodePath,
            string rootDirectoryFullName,
            string diretoryToLoadFullName
        )
        {
            NodePath = nodePath;
            RootDirectoryFullName = rootDirectoryFullName;
            DirectoryToLoadFullName = diretoryToLoadFullName;
            NodeType = string.Empty;
        }

        public QueryForEditorNodeFromPath(
            IList<string> nodePath,
            string rootDirectoryFullName,
            string diretoryToLoadFullName,
            string nodeType
        )
        {
            NodePath = nodePath;
            RootDirectoryFullName = rootDirectoryFullName;
            DirectoryToLoadFullName = diretoryToLoadFullName;
            NodeType = nodeType;
        }
    }
}
