namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.State
{
    public partial struct BehaviorTreeState
    {
        public BehaviorTreeState AddActiveTraversalToNextStack()
        {
            NextTraversalStack.Add(
                _activeTraversalToken
            );
            return this;
        }
    }
}
