using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.System.Editor.Model.Builder;
using EventHorizon.Zone.System.Editor.Events;
using EventHorizon.Zone.System.Editor.Model;
using MediatR;

namespace EventHorizon.Zone.System.Combat.Plugin.Editor.Editor
{
    public struct FillClientSkillsEditorNodeStateHandler : INotificationHandler<FillEditorNodeState>
    {
        readonly ServerInfo _serverInfo;

        public FillClientSkillsEditorNodeStateHandler(
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
                    // Disable context menu support.
                    EditorNodePropertySupportKeys.SUPPORT_CONTEXT_MENU_KEY,
                    false
                ).AddChild(
                    // Add the skill node as a child to it.
                    await CreateSkillNode(
                        rootFolder
                    )
                )
            );
        }

        private async Task<IEditorNode> CreateSkillNode(
            string rootFolder
        )
        {
            // Create Script Node
            return (await LoadEditorNodeFromPath.Create(
                rootFolder,
                _serverInfo.ClientPath,
                Path.Combine(
                    _serverInfo.ClientPath,
                    "Skills"
                ), 
                "EDITOR_SKILL"
            )).AddProperty(
                EditorNodePropertySupportKeys.SUPPORT_DELETE_KEY,
                false
            );
        }
    }
}