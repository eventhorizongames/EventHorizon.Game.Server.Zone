using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.System.Editor.Model.Builder;
using EventHorizon.Zone.System.Editor.Events;
using EventHorizon.Zone.System.Editor.Model;
using MediatR;

namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Editor.Fill
{
    public class FillClientAgentBehaviorEditorNodeStateHandler : INotificationHandler<FillEditorNodeState>
    {
        readonly ServerInfo _serverInfo;

        public FillClientAgentBehaviorEditorNodeStateHandler(
            ServerInfo serverInfo
        )
        {
            _serverInfo = serverInfo;
        }

        public async Task Handle(
            FillEditorNodeState notification,
            CancellationToken cancellationToken
        )
        {
            // Node Root Folder Details
            var rootFolder = "Server";

            notification.AddNode(
                // Create the root node.
                new StandardEditorNode(
                    rootFolder
                ).AddProperty(
                    // Disable context menu support on the root node.
                    EditorNodePropertySupportKeys.SUPPORT_CONTEXT_MENU_KEY,
                    false
                ).AddChild(
                    await CreateNode(
                        rootFolder
                    )
                )
            );
        }

        private async Task<IEditorNode> CreateNode(
            string rootFolder
        )
        {
            // Create Script Node
            return (await LoadEditorNodeFromPath.Create(
                rootFolder,
                _serverInfo.ServerPath,
                Path.Combine(
                    _serverInfo.ServerPath,
                    "Behaviors"
                )
            )).AddProperty(
                EditorNodePropertySupportKeys.SUPPORT_DELETE_KEY, 
                false
            );
        }
    }
}