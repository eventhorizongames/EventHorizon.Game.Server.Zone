namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.State;

public partial struct BehaviorTreeState
{
    public BehaviorTreeState PushActiveNodeToTraversalStack()
    {
        TraversalStack.Add(
            _activeNodeToken
        );
        _activeTraversalToken = _activeNodeToken;
        return this;
    }
}
