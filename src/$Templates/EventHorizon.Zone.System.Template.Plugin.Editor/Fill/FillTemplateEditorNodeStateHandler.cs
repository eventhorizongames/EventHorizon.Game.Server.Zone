namespace EventHorizon.Zone.System.Template.Plugin.Editor.Fill;

using global::System.IO;
using global::System.Threading;
using global::System.Threading.Tasks;

using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.System.Editor.Events;
using EventHorizon.Zone.System.Editor.Events.Node;
using EventHorizon.Zone.System.Editor.Model;

using MediatR;

public class FillTemplateEditorNodeStateHandler
    : INotificationHandler<FillEditorNodeState>
{
    readonly ISender _sender;
    readonly ServerInfo _serverInfo;

    public FillTemplateEditorNodeStateHandler(
        ISender sender,
        ServerInfo serverInfo
    )
    {
        _sender = sender;
        _serverInfo = serverInfo;
    }

    public async Task Handle(
        FillEditorNodeState notification,
        CancellationToken cancellationToken
    )
    {
        // Node Root Folder Details
        var rootFolder = "Template";

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
        return (await _sender.Send(
            new QueryForEditorNodeFromPath(
                new string[] { rootFolder },
                _serverInfo.SystemsPath,
                Path.Combine(
                    _serverInfo.SystemsPath,
                    "Template"
                )
            )
        )).AddProperty(
            EditorNodePropertySupportKeys.SUPPORT_DELETE_KEY,
            false
        );
    }
}
