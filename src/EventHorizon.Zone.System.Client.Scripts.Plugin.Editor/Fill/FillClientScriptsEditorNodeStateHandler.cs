namespace EventHorizon.Zone.System.Client.Scripts.Plugin.Editor.Fill
{
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.System.Editor.Events;
    using EventHorizon.Zone.System.Editor.Events.Node;
    using EventHorizon.Zone.System.Editor.Model;
    using MediatR;

    public class FillClientScriptsEditorNodeStateHandler 
        : INotificationHandler<FillEditorNodeState>
    {
        readonly IMediator _mediator;
        readonly ServerInfo _serverInfo;

        public FillClientScriptsEditorNodeStateHandler(
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
            var node = (await _mediator.Send(
                new QueryForEditorNodeFromPath(
                    new string[] { rootFolder },
                    _serverInfo.ClientPath,
                    _serverInfo.ClientScriptsPath
                )
            )).AddProperty(
                EditorNodePropertySupportKeys.SUPPORT_DELETE_KEY,
                false
            );

            //foreach (var child in node.Children)
            //{
            //    child.AddProperty(
            //        EditorNodePropertySupportKeys.SUPPORT_DELETE_KEY,
            //        false
            //    );
            //}

            return node;
        }
    }
}