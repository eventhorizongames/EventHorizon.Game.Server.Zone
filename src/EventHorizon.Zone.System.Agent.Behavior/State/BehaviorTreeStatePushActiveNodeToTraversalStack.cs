using System.Linq;
using System.Collections.Generic;
using EventHorizon.Zone.System.Agent.Behavior.Model;

namespace EventHorizon.Zone.System.Agent.Behavior.State
{
    public partial struct BehaviorTreeState
    {
        public BehaviorTreeState PushActiveNodeToTraversalStack()
        {
            this.TraversalStack.Add(
                _activeNodeToken
            );
            _activeTraversalToken = _activeNodeToken;
            return this;
        }
    }
}