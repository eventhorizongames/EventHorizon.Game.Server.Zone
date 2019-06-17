using System.Linq;
using System.Collections.Generic;
using EventHorizon.Zone.System.Agent.Behavior.Model;

namespace EventHorizon.Zone.System.Agent.Behavior.State
{
    public partial struct BehaviorTreeState
    {
        public BehaviorTreeState ActivateNode(
            int token
        )
        {
            this._activeNodeToken = token;
            return this;
        }
    }
}