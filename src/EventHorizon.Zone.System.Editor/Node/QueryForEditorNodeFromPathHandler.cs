using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Editor.Events.Node;
using EventHorizon.Zone.System.Editor.Model;
using MediatR;
using EventHorizon.Zone.System.Editor.Model.Builder;

namespace EventHorizon.Zone.System.Editor.Node
{
    public class QueryForEditorNodeFromPathHandler : IRequestHandler<QueryForEditorNodeFromPath, IEditorNode>
    {
        public Task<IEditorNode> Handle(
            QueryForEditorNodeFromPath request,
            CancellationToken cancellationToken
        ) => LoadEditorNodeFromPath.Create(
            request.RootFolderName,
            request.RootFolderPath,
            request.DirectoryToLoadPath,
            request.NodeType
        );
    }
}