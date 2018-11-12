
using System.Collections.Concurrent;
using EventHorizon.Plugin.Zone.System.Combat.Skill.Model;

namespace EventHorizon.Plugin.Zone.System.Combat.Skill.State
{
    public interface ISkillValidatorScriptRepository
    {
        void Add(SkillValidatorScript script);
        SkillValidatorScript Find(string id);
    }

    public class SkillValidatorScriptRepository : ISkillValidatorScriptRepository
    {
        private static readonly ConcurrentDictionary<string, SkillValidatorScript> SCRIPT_MAP = new ConcurrentDictionary<string, SkillValidatorScript>();
        public void Add(SkillValidatorScript script)
        {
            SCRIPT_MAP.AddOrUpdate(script.Id, script, (key, old) => script);
        }
        public SkillValidatorScript Find(string id)
        {
            var script = default(SkillValidatorScript);
            SCRIPT_MAP.TryGetValue(id, out script);
            return script;
        }
    }
}