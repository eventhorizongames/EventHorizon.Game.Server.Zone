namespace EventHorizon.Zone.System.Editor.Model
{
    using global::System.Collections.Generic;

    public interface IEditorNode
    {
        string Id { get; }
        string Name { get; }
        bool IsFolder { get; }
        IList<string> Path { get; }
        string Type { get; }
        IList<IEditorNode> Children { get; }
        IDictionary<string, object> Properties { get; }
        IEditorNode AddProperty(
            string key,
            object value
        );
        IEditorNode AddChild(
            IEditorNode child
        );
    }
}
