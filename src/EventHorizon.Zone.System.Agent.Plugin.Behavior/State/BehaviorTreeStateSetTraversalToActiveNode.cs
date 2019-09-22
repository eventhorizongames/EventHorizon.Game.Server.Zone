namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.State
{
    public partial struct BehaviorTreeState
    {
        /// <summary>
        /// Flag this state to check traversal the next time it has a chance to.
        /// </summary>
        /// <returns>Update Tree State.</returns>
        public BehaviorTreeState SetTraversalToCheck()
        {
            return this.SetCheckTraversal(
                true
            );
        }
    }
}