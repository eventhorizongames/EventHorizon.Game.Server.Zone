using System.Collections.Generic;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;

namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.State
{
    public partial struct BehaviorTreeState
    {
        public IList<BehaviorNode> GetActiveTraversalChildren()
        {
            // TODO: Optimize this by making it a dictionary lookup
            return _shape.GetChildren(_activeTraversalToken);
        }
    }
}