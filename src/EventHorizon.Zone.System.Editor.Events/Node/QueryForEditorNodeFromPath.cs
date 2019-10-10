using EventHorizon.Zone.System.Editor.Model;
using EventHorizon.Zone.System.Editor.Model.Builder;
using MediatR;

namespace EventHorizon.Zone.System.Editor.Events.Node
{
    public struct QueryForEditorNodeFromPath : IRequest<IEditorNode>
    {
        public string RootFolderName { get; }
        public string RootFolderPath { get; }
        public string DirectoryToLoadPath { get; }
        public string NodeType { get; }
        
        public QueryForEditorNodeFromPath(
            string rootFolderName,
            string rootFolderPath,
            string diretoryToLoadPath
        )
        {
            RootFolderName = rootFolderName;
            RootFolderPath = rootFolderPath;
            DirectoryToLoadPath = diretoryToLoadPath;
            NodeType = LoadEditorNodeFromPath.DEFAULT_NODE_TYPE;
        }
        public QueryForEditorNodeFromPath(
            string rootFolderName,
            string rootFolderPath,
            string diretoryToLoadPath,
            string nodeType
        )
        {
            RootFolderName = rootFolderName;
            RootFolderPath = rootFolderPath;
            DirectoryToLoadPath = diretoryToLoadPath;
            NodeType = nodeType;
        }
    }
}