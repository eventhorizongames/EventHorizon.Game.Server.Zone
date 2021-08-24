namespace EventHorizon.Zone.System.Editor.Events
{
    using EventHorizon.Zone.System.Editor.Model;

    using MediatR;

    public struct FillEditorNodeState : INotification
    {
        public IEditorNodeList EditorState { get; }
        public void AddNode(
            IEditorNode node
        )
        {
            EditorState.AddNode(
                node
            );
        }

        public FillEditorNodeState(
            IEditorNodeList editorState
        )
        {
            EditorState = editorState;
        }
    }
}
