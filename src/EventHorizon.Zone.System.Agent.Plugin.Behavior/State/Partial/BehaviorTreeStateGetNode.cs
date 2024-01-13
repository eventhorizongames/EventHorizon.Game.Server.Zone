namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.State;

using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;

public partial struct BehaviorTreeState
{
    public BehaviorNode GetNode(
        int token
    )
    {
        if (token == -1)
        {
            return default;
        }
        if (!NodeMap.ContainsKey(
            token
        ))
        {
            NodeMap[token] = _shape.GetNode(
                token
            );
        }
        return NodeMap[token];
    }
}
