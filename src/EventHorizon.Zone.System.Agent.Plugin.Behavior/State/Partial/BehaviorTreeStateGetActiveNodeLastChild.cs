using System.Linq;

namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.State
{
    public partial struct BehaviorTreeState
    {
        public int GetActiveNodeLastChild()
        {
            return _shape.GetChildren(
                this._activeNodeToken
            ).Last().Token;
        }
    }
}