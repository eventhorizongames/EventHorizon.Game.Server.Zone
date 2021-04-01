namespace EventHorizon.Zone.System.Wizard.Plugin.Editor.Fill
{
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.System.Editor.Events;
    using EventHorizon.Zone.System.Editor.Events.Node;
    using EventHorizon.Zone.System.Editor.Model;
    using global::System.IO;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;

    public class FillWizardEditorNodeStateHandler
        : INotificationHandler<FillEditorNodeState>
    {
        private readonly IMediator _mediator;
        private readonly ServerInfo _serverInfo;

        public FillWizardEditorNodeStateHandler(
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
            var rootFolder = "Server";
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
                    await CreateWizardsNode(
                        rootFolder
                    )
                )
            );
        }

        private async Task<IEditorNode> CreateWizardsNode(
            string rootFolder
        )
        {
            var node = (await _mediator.Send(
                new QueryForEditorNodeFromPath(
                    new string[] { rootFolder },
                    _serverInfo.ServerPath,
                    Path.Combine(
                        _serverInfo.ServerPath,
                        "Wizards"
                    )
                )
            )).AddProperty(
                EditorNodePropertySupportKeys.SUPPORT_CONTEXT_MENU_KEY,
                false
            );

            foreach (var child in node.Children)
            {
                child.AddProperty(
                    EditorNodePropertySupportKeys.SUPPORT_DELETE_KEY,
                    false
                );
            }

            return node;
        }
    }
}