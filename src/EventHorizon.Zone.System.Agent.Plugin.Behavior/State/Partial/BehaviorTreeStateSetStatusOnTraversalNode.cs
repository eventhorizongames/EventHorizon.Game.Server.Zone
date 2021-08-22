namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.State
{
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;

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
