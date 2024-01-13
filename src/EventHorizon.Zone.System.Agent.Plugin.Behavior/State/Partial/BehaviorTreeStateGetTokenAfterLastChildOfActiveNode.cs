
namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.State;

using global::System.Linq;

public partial struct BehaviorTreeState
{
    public int GetTokenAfterLastChildOfActiveNode()
    {
        var lastChildToken = _shape.GetChildren(
            _activeNodeToken
        ).Last().Token;
        return GetNodeTokenAfterPassedToken(
            lastChildToken
        );
    }

    private int GetNodeTokenAfterPassedToken(
        int token
    )
    {
        for (int i = 0; i < ShapeOrder.Length; i++)
        {
            if (ShapeOrder[i] == token && (i + 1 != ShapeOrder.Length))
            {
                return ShapeOrder[i + 1];
            }
        }
        return -1;
    }
}
