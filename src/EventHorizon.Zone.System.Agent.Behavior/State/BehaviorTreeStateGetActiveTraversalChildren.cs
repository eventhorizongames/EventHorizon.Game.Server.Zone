using System.Linq;
using System.Collections.Generic;
using EventHorizon.Zone.System.Agent.Behavior.Model;

namespace EventHorizon.Zone.System.Agent.Behavior.State
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