using System.Linq;
using System.Collections.Generic;
using EventHorizon.Zone.System.Agent.Behavior.Model;

namespace EventHorizon.Zone.System.Agent.Behavior.State
{
    public partial struct BehaviorTreeState
    {
        public BehaviorTreeState SetNextChildNodeOfStatusToActive(
            BehaviorNodeStatus status
        )
        {
            if (!this.ContainsNext)
            {
                return this;
            }
            // Look for first READY Node
            foreach (var activeChildNode in _shape.GetChildren(_activeTraversalToken))
            {
                // If this child is Ready
                if (status.Equals(
                    GetNode(
                        activeChildNode.Token
                    ).Status
                ))
                {
                    // Set as next active node
                    ActivateNode(
                       activeChildNode.Token
                    );
                    // Break out so this node can be processed.
                    break;
                }
            }
            return this;
        }
    }
}