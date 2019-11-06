using EventHorizon.Zone.System.Editor.Model;
using MediatR;

namespace EventHorizon.Zone.System.Editor.Events.State
{
    public struct GetEditorState : IRequest<IEditorNodeList>
    {
    }
}