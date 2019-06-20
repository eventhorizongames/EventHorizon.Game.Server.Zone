using EventHorizon.Zone.System.Agent.Behavior.Script;

namespace EventHorizon.Zone.System.Agent.Behavior.Api
{
    public interface ActorBehaviorScriptRepository
    {
        void Clear();
        void Add(
            BehaviorScript script
        );
        BehaviorScript Find(
            string id
        );
    }
}