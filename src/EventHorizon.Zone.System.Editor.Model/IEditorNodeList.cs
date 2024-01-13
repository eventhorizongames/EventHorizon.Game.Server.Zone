namespace EventHorizon.Zone.System.Editor.Model;

using global::System.Collections.Generic;

public interface IEditorNodeList
{
    IList<IEditorNode> Root { get; }

    void AddNode(
        IEditorNode node
    );
}
