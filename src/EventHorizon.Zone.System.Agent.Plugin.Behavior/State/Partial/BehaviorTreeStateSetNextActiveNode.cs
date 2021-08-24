namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.State
{
    public partial struct BehaviorTreeState
    {
        public BehaviorTreeState SetNextActiveNode()
        {
            return PopActiveNodeFromQueue();
        }
    }
}
