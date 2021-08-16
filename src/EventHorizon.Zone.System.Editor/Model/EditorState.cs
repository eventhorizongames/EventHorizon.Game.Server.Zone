namespace EventHorizon.Zone.System.Editor.Model
{
    using global::System.Collections.Generic;
    using global::System.Linq;

    public class EditorState : IEditorNodeList
    {
        public IList<IEditorNode> Root { get; } = new List<IEditorNode>();

        public void AddNode(
            IEditorNode node
        )
        {
            var foundNode = Root.FirstOrDefault(
                a => a.Name == node.Name
            );
            if (foundNode != null)
            {
                foreach (var child in node.Children)
                {
                    foundNode.Children.Add(
                        child
                    );
                }
                return;
            }

            Root.Add(
                node
            );
        }
    }
}
