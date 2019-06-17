using System.Collections.Concurrent;
using EventHorizon.Zone.System.Agent.Behavior.Api;
using EventHorizon.Zone.System.Agent.Behavior.Script;

namespace EventHorizon.Zone.System.Agent.Behavior.State
{
    public class InMemoryAgentBehaviorScriptRepository : AgentBehaviorScriptRepository
    {
        private static readonly ConcurrentDictionary<string, BehaviorScript> SCRIPT_MAP = new ConcurrentDictionary<string, BehaviorScript>();
        public void Add(
            BehaviorScript script
        )
        {
            SCRIPT_MAP.AddOrUpdate(script.Id, script, (key, old) => script);
        }
        public BehaviorScript Find(
            string id
        )
        {
            var result = default(BehaviorScript);
            SCRIPT_MAP.TryGetValue(id, out result);
            return result;
        }
    }
}