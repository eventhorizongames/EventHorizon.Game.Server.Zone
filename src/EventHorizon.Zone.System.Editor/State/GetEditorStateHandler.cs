namespace EventHorizon.Zone.System.Editor.State;

using EventHorizon.Zone.System.Editor.Events;
using EventHorizon.Zone.System.Editor.Events.State;
using EventHorizon.Zone.System.Editor.Model;

using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

public class GetEditorStateHandler : IRequestHandler<GetEditorState, IEditorNodeList>
{
    private readonly IMediator _mediator;

    public GetEditorStateHandler(
        IMediator mediator
    )
    {
        _mediator = mediator;
    }

    public async Task<IEditorNodeList> Handle(
        GetEditorState request,
        CancellationToken cancellationToken
    )
    {
        var editorState = new EditorState();
        await _mediator.Publish(
            new FillEditorNodeState(
                editorState
            )
        );
        return editorState;
    }
}
