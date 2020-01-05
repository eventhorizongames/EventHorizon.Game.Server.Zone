using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.System.Editor.Events;
using EventHorizon.Zone.System.Editor.Model;
using MediatR;
using EventHorizon.Zone.System.Editor.Events.Node;

namespace EventHorizon.Zone.System.EntityModule.Plugin.Editor.Fill
{
    public class FillEntityModuleEditorNodeStateHandler : INotificationHandler<FillEditorNodeState>
    {
        readonly IMediator _mediator;
        readonly ServerInfo _serverInfo;

        public FillEntityModuleEditorNodeStateHandler(
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
            // Node Root Folder Details
            var rootFolder = "Client";
            var modulesFolder = "Modules";

            notification.AddNode(
                // Create the root node.
                new StandardEditorNode(
                    rootFolder
                ).AddProperty(
                    // Disable context menu support on the root node.
                    EditorNodePropertySupportKeys.SUPPORT_CONTEXT_MENU_KEY,
                    false
                ).AddChild(
                    new StandardEditorNode(
                        modulesFolder,
                        true,
                        new string[] { rootFolder },
                        "FOLDER" // TODO: Move this into a constants class
                    ).AddProperty(
                        // Disable context menu support on the root node.
                        EditorNodePropertySupportKeys.SUPPORT_CONTEXT_MENU_KEY,
                        false
                    ).AddChild(
                        await CreateBaseNode(
                            rootFolder,
                            modulesFolder
                        )
                    ).AddChild(
                        await CreatePlayerNode(
                            rootFolder,
                            modulesFolder
                        )
                    )
                )
            );
        }

        private async Task<IEditorNode> CreateBaseNode(
            string rootFolder,
            string modulesFolder
        )
        {
            return (await _mediator.Send(
                new QueryForEditorNodeFromPath(
                    new List<string> { rootFolder, modulesFolder },
                    _serverInfo.ClientPath,
                    Path.Combine(
                        _serverInfo.ClientPath,
                        modulesFolder,
                        "Base"
                    )
                )
            )).AddProperty(
                EditorNodePropertySupportKeys.SUPPORT_DELETE_KEY,
                false
            );
        }

        private async Task<IEditorNode> CreatePlayerNode(
            string rootFolder,
            string modulesFolder
        )
        {
            return (await _mediator.Send(
                new QueryForEditorNodeFromPath(
                    new List<string> { rootFolder, modulesFolder },
                    _serverInfo.ClientPath,
                    Path.Combine(
                        _serverInfo.ClientPath,
                        modulesFolder,
                        "Player"
                    )
                )
            )).AddProperty(
                EditorNodePropertySupportKeys.SUPPORT_DELETE_KEY,
                false
            );
        }
    }
}