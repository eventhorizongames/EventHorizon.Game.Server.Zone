using System.Linq;
using System.Collections.Generic;
using EventHorizon.Zone.System.Agent.Behavior.Model;

namespace EventHorizon.Zone.System.Agent.Behavior.State
{
    public partial struct BehaviorTreeState
    {
        /// <summary>
        /// Flag this state to check traversal the next time it has a chance to.
        /// </summary>
        /// <returns>Update Tree State.</returns>
        public BehaviorTreeState SetTraversalToCheck()
        {
            return this.SetCheckTraversal(
                true
            );
        }
    }
}