using System;
using System.Collections.Concurrent;
using EventHorizon.Zone.Plugin.Interaction.Script.Api;

namespace EventHorizon.Zone.Plugin.Interaction.Script.State
{
    public interface InteractionScriptRepository
    {
        void Set(
            InteractionScript script
        );
        InteractionScript Get(
            string scriptId
        );
    }
    public class InMemoryInteractionScriptRepository : InteractionScriptRepository
    {
        private static readonly ConcurrentDictionary<string, InteractionScript> INTERNAL_MAP = new ConcurrentDictionary<string, InteractionScript>();
        
        public void Set(
            InteractionScript script
        )
        {
            INTERNAL_MAP.AddOrUpdate(
                script.Id,
                script,
                (key, old) => script
            );
        }

        public InteractionScript Get(string scriptId)
        {
            var script = default(InteractionScript);
            if (!INTERNAL_MAP.TryGetValue(
                scriptId,
                out script
            ))
            {
                throw new ArgumentException(
                    "Script Id invalid",
                    "scriptId"
                );
            }
            return script;
        }
    }
}