using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;

namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.State
{
    public partial struct BehaviorTreeState
    {
        public BehaviorTreeState SetStatusOnTraversalNode(
            BehaviorNodeStatus status
        )
        {
            this.NodeMap[_activeTraversalToken] = ActiveTraversal.UpdateStatus(
                status.ToString()
            );
            return this;
        }
    }
}
