namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Update
{
    using MediatR;
    
    public struct RunBehaviorTreeUpdate : INotification
    {
        public string TreeId { get; }
        public RunBehaviorTreeUpdate(
            string treeId
        ) => TreeId = treeId;
    }
}