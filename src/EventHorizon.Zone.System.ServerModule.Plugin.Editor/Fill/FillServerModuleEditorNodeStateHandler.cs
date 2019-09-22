using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.System.Editor.Model.Builder;
using EventHorizon.Zone.System.Editor.Events;
using EventHorizon.Zone.System.Editor.Model;
using MediatR;

namespace EventHorizon.Zone.System.ServerModule.Plugin.Editor
{
    public struct FillServerModuleEditorNodeStateHandler : INotificationHandler<FillEditorNodeState>
    {
        readonly ServerInfo _serverInfo;

        public FillServerModuleEditorNodeStateHandler(
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
            var rootFolder = "Client";

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
                _serverInfo.ClientPath,
                Path.Combine(
                    _serverInfo.ClientPath,
                    "ServerModule"
                )
            )).AddProperty(
                EditorNodePropertySupportKeys.SUPPORT_DELETE_KEY,
                false
            );
        }
    }
}