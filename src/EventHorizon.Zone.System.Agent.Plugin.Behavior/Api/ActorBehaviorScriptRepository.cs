using EventHorizon.Zone.System.Agent.Plugin.Behavior.Script;

namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Api
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