using System.Collections.Generic;
using System.Linq;

namespace EventHorizon.Zone.System.Editor.Model
{
    public class EditorState : IEditorNodeList
    {
        public IList<IEditorNode> Root { get; } = new List<IEditorNode>();

        public void AddNode(
            IEditorNode node
        )
        {
            Root.Add(
                node
            );
        }
    }
}