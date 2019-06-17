using System.Linq;
using System.Collections.Generic;
using EventHorizon.Zone.System.Agent.Behavior.Model;

namespace EventHorizon.Zone.System.Agent.Behavior.State
{
    public partial struct BehaviorTreeState
    {
        public BehaviorTreeState SetNextRunningNodeToActive()
        {
            return SetNextChildNodeOfStatusToActive(
                BehaviorNodeStatus.RUNNING
            );
        }
    }
}