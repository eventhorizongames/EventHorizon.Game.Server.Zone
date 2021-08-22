namespace EventHorizon.Zone.System.Editor.Events.State
{
    using EventHorizon.Zone.System.Editor.Model;

    using MediatR;

    public struct GetEditorState : IRequest<IEditorNodeList>
    {
    }
}
