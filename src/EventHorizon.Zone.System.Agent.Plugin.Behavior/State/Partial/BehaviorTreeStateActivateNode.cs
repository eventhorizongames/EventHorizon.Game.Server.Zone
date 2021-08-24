namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.State
{
    public partial struct BehaviorTreeState
    {
        public BehaviorTreeState ActivateNode(
            int token
        )
        {
            _activeNodeToken = token;
            return this;
        }
    }
}
