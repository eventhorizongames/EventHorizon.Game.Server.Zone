using System.Collections.Concurrent;
using System.Collections.Generic;
using EventHorizon.Plugin.Zone.System.Combat.Skill.Model;

namespace EventHorizon.Plugin.Zone.System.Combat.Skill.State
{
    public interface ISkillActionScriptRepository
    {
        void Add(SkillActionScript script);
        SkillActionScript Find(string id);
        IEnumerable<SkillActionScript> All();
    }

    public class SkillActionScriptRepository : ISkillActionScriptRepository
    {
        private static readonly ConcurrentDictionary<string, SkillActionScript> SCRIPT_MAP = new ConcurrentDictionary<string, SkillActionScript>();
        public void Add(SkillActionScript script)
        {
            SCRIPT_MAP.AddOrUpdate(script.Id, script, (key, old) => script);
        }
        public SkillActionScript Find(string id)
        {
            var script = default(SkillActionScript);
            SCRIPT_MAP.TryGetValue(id, out script);
            return script;
        }
        public IEnumerable<SkillActionScript> All()
        {
            return SCRIPT_MAP.Values;
        }
    }
}