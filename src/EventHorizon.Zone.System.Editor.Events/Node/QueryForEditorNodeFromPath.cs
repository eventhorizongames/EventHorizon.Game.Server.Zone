using System.Collections.Generic;

using EventHorizon.Zone.System.Editor.Model;

using MediatR;

namespace EventHorizon.Zone.System.Editor.Events.Node
{
    public struct QueryForEditorNodeFromPath : IRequest<IEditorNode>
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
            NodeType = null;
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
