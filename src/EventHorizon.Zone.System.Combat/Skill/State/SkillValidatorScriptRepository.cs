
using System.Collections.Concurrent;
using System.Collections.Generic;
using EventHorizon.Zone.System.Combat.Skill.Model;

namespace EventHorizon.Zone.System.Combat.Skill.State
{
    public interface ISkillValidatorScriptRepository
    {
        void Add(SkillValidatorScript script);
        IEnumerable<SkillValidatorScript> All();
        SkillValidatorScript Find(string id);
    }

    public class SkillValidatorScriptRepository : ISkillValidatorScriptRepository
    {
        private static readonly ConcurrentDictionary<string, SkillValidatorScript> SCRIPT_MAP = new ConcurrentDictionary<string, SkillValidatorScript>();
        public void Add(SkillValidatorScript script)
        {
            SCRIPT_MAP.AddOrUpdate(script.Id, script, (key, old) => script);
        }

        public IEnumerable<SkillValidatorScript> All()
        {
            return SCRIPT_MAP.Values;
        }

        public SkillValidatorScript Find(string id)
        {
            var script = default(SkillValidatorScript);
            SCRIPT_MAP.TryGetValue(id, out script);
            return script;
        }
    }
}