using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.System.Editor.Events;
using EventHorizon.Zone.System.Editor.Events.Node;
using EventHorizon.Zone.System.Editor.Model;
using MediatR;
using IOPath = System.IO.Path;

namespace EventHorizon.Zone.System.Admin.Plugin.Command.Scripts.Editor
{
    public struct FillAdminCommandsEditorNodeStateHandler : INotificationHandler<FillEditorNodeState>
    {
        readonly IMediator _mediator;
        readonly ServerInfo _serverInfo;

        public FillAdminCommandsEditorNodeStateHandler(
            IMediator mediator,
            ServerInfo serverInfo
        )
        {
            _mediator = mediator;
            _serverInfo = serverInfo;
        }

        public async Task Handle(
            FillEditorNodeState notification,
            CancellationToken cancellationToken
        )
        {
            var rootFolder = "Admin";
            notification.AddNode(
                // Create the root node.
                new StandardEditorNode(
                    rootFolder
                ).AddProperty(
                    // Disable context menu support.
                    EditorNodePropertySupportKeys.SUPPORT_CONTEXT_MENU_KEY,
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
            return (await _mediator.Send(
                new QueryForEditorNodeFromPath(
                    rootFolder,
                    _serverInfo.AdminPath,
                    IOPath.Combine(
                        _serverInfo.AdminPath,
                        "Commands"
                    )
                )
            )).AddProperty(
                EditorNodePropertySupportKeys.SUPPORT_DELETE_KEY,
                false
            );
        }
    }
}