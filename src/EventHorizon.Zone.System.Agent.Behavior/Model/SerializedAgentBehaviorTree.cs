
namespace EventHorizon.Zone.System.Agent.Behavior.Model
{
    public struct SerializedAgentBehaviorTree
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public SerializedBehaviorNode Root { get; set; }
    }
}