using System.Collections.Generic;
using System.Linq;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;

namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.State
{
    public partial struct BehaviorTreeState
    {
        public int GetTokenAfterLastChildOfTraversalNode()
        {
            var children = GetActiveTraversalChildren();
            if (children.Count > 0)
            {
                return GetNodeTokenAfterPassedToken(
                    GetActiveTraversalChildren()
                        .Last().Token
                );
            }
            return -1;
        }
        
        public IList<BehaviorNode> GetActiveTraversalChildren()
        {
            return _shape.GetChildren(
                _activeTraversalToken
            );
        }
    }
}