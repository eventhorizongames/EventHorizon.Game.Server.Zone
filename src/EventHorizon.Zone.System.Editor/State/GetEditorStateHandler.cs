using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Editor.Events;
using EventHorizon.Zone.System.Editor.Events.State;
using EventHorizon.Zone.System.Editor.Model;
using MediatR;

namespace EventHorizon.Zone.System.Editor.State
{
    public class GetEditorStateHandler : IRequestHandler<GetEditorState, IEditorNodeList>
    {
        readonly IMediator _mediator;
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
}