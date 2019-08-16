using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.External.Info;
using EventHorizon.Zone.Plugin.Editor.Builder;
using EventHorizon.Zone.System.Editor.Events;
using EventHorizon.Zone.System.Editor.Model;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Server.Editor
{
    public struct FillServerScriptsEditorNodeStateHandler : INotificationHandler<FillEditorNodeState>
    {
        readonly ServerInfo _serverInfo;

        public FillServerScriptsEditorNodeStateHandler(
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
            var rootFolder = "Server";
            notification.AddNode(
                // Create the root node.
                new StandardEditorNode(
                    rootFolder
                ).AddProperty(
                    // Disable context menu support.
                    "support:contextMenu",
                    false
                ).AddChild(
                    // Add the script node as a child to it.
                    await CreateScriptNode(
                        rootFolder
                    )
                )
            );
        }

        private async Task<IEditorNode> CreateScriptNode(
            string rootFolder
        )
        {
            var node = (await LoadEditorNodeFromPath.Create(
                rootFolder,
                _serverInfo.ServerPath,
                _serverInfo.ServerScriptsPath
            )).AddProperty(
                "support:contextMenu", 
                false
            );

            foreach (var child in node.Children)
            {
                child.AddProperty("support:delete", false);
            }

            return node;
        }
    }
}