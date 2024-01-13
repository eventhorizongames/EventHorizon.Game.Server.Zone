namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.State;

using global::System.Linq;

public partial struct BehaviorTreeState
{
    public BehaviorTreeState PopActiveTraversalNode()
    {
        TraversalStack.Remove(
            _activeTraversalToken
        );
        if (ContainsNext)
        {
            _activeTraversalToken = TraversalStack.Last();
        }
        else
        {
            _activeTraversalToken = -1;
            _checkTraversal = false;
        }
        return this;
    }
}
