using EventHorizon.Zone.System.Agent.Behavior.Script;

namespace EventHorizon.Zone.System.Agent.Behavior.Api
{
    public interface AgentBehaviorScriptRepository
    {
        void Add(
            BehaviorScript script
        );
        BehaviorScript Find(
            string id
        );
    }
}