namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.State
{
    using global::System.Linq;

    public partial struct BehaviorTreeState
    {
        public BehaviorTreeState PopActiveTraversalNode()
        {
            this.TraversalStack.Remove(
                this._activeTraversalToken
            );
            if (ContainsNext)
            {
                this._activeTraversalToken = this.TraversalStack.Last();
            }
            else
            {
                this._activeTraversalToken = -1;
                this._checkTraversal = false;
            }
            return this;
        }
    }
}
