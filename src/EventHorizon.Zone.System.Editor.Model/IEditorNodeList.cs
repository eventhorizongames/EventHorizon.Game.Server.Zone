using System.Collections.Generic;

namespace EventHorizon.Zone.System.Editor.Model
{
    public interface IEditorNodeList
    {
        IList<IEditorNode> Root { get; }
        
        void AddNode(
            IEditorNode node
        );
    }
}