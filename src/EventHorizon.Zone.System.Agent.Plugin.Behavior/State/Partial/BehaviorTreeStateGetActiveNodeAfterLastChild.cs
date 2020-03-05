using System.Linq;

namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.State
{
    public partial struct BehaviorTreeState
    {
        // TODO: Fix name GetTokenAfterLastChildOfActiveNode
        public int GetTokenAfterLastChildOfActiveNode()
        {
            var lastChildToken = _shape.GetChildren(
                this._activeNodeToken
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
}