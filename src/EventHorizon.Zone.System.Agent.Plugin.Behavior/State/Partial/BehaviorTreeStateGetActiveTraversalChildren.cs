using System.Collections.Generic;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;

namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.State
{
    public partial struct BehaviorTreeState
    {
        public IList<BehaviorNode> GetActiveTraversalChildren()
        {
            return _shape.GetChildren(_activeTraversalToken);
        }
    }
}