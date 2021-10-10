namespace EventHorizon.Zone.System.Player.Plugin.Editor.Fill
{
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.System.Editor.Events;
    using EventHorizon.Zone.System.Editor.Events.Node;
    using EventHorizon.Zone.System.Editor.Model;

    using global::System;
    using global::System.IO;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    public class FillPlayerEditorNodeStateHandler
        : INotificationHandler<FillEditorNodeState>
    {
        private readonly IMediator _mediator;
        private readonly ServerInfo _serverInfo;

        public FillPlayerEditorNodeStateHandler(
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
            var node = await CreateNode(
                cancellationToken
            );

            foreach (var child in node.Children)
            {
                child.AddProperty(
                    EditorNodePropertySupportKeys.SUPPORT_CONTEXT_MENU_KEY,
                    false
                );
            }

            notification.AddNode(
                node
            );
        }

        private async Task<IEditorNode> CreateNode(
            CancellationToken cancellationToken
        ) => (await _mediator.Send(
            new QueryForEditorNodeFromPath(
                Array.Empty<string>(),
                "Player",
                Path.Combine(
                    _serverInfo.AppDataPath,
                    "Player"
                )
            ),
            cancellationToken
        )).AddProperty(
            EditorNodePropertySupportKeys.SUPPORT_CONTEXT_MENU_KEY,
            false
        );
    }
}
