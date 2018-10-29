using System.Collections.Concurrent;
using EventHorizon.Plugin.Zone.System.Combat.Skill.Model;

namespace EventHorizon.Plugin.Zone.System.Combat.Skill.State
{
    public interface ISkillEffectScriptRepository
    {
        void Add(SkillEffectScript script);
        SkillEffectScript Find(string id);
    }

    public class SkillEffectScriptRepository : ISkillEffectScriptRepository
    {
        private static readonly ConcurrentDictionary<string, SkillEffectScript> SCRIPT_MAP = new ConcurrentDictionary<string, SkillEffectScript>();
        public void Add(SkillEffectScript script)
        {
            SCRIPT_MAP.AddOrUpdate(script.Id, script, (key, old) => script);
        }
        public SkillEffectScript Find(string id)
        {
            var script = default(SkillEffectScript);
            SCRIPT_MAP.TryGetValue(id, out script);
            return script;
        }
    }
}