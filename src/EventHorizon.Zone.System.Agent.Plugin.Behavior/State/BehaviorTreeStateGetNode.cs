using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;

namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.State
{
    public partial struct BehaviorTreeState
    {
        public BehaviorNode GetNode(
            int token
        )
        {
            if (token == -1)
            {
                return default(BehaviorNode);
            }
            if (!this.NodeMap.ContainsKey(
                token
            ))
            {
                this.NodeMap[token] = this._shape.GetNode(
                    token
                );
            }
            return this.NodeMap[token];
        }
        
    }
}