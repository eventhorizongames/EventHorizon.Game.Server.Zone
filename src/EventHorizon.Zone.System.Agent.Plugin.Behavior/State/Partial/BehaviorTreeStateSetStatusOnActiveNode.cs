using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;

namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.State
{
    public partial struct BehaviorTreeState
    {
        public BehaviorTreeState SetStatusOnActiveNode(
            BehaviorNodeStatus status
        )
        {
            this.NodeMap[_activeNodeToken] = ActiveNode.UpdateStatus(
                status.ToString()
            );
            return this;
        }
    }
}