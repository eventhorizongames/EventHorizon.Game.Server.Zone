using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Editor.Model;
using MediatR;

namespace EventHorizon.Zone.System.Editor.Events
{
    public struct GetEditorState : IRequest<EditorState>
    {
        public struct GetEditorStateHandler : IRequestHandler<GetEditorState, EditorState>
        {
            readonly IMediator _mediator;
            public GetEditorStateHandler(
                IMediator mediator
            )
            {
                _mediator = mediator;
            }
            public async Task<EditorState> Handle(
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
}