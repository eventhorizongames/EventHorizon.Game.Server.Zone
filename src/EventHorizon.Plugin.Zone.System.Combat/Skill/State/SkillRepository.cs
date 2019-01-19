using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using EventHorizon.Plugin.Zone.System.Combat.Skill.Model;

namespace EventHorizon.Plugin.Zone.System.Combat.Skill.State
{
    public interface ISkillRepository
    {
        IList<SkillInstance> All();
        SkillInstance Find(string id);
        void Add(SkillInstance skill);
    }

    public class SkillRepository : ISkillRepository
    {
        private static readonly ConcurrentDictionary<string, SkillInstance> SCRIPT_MAP = new ConcurrentDictionary<string, SkillInstance>();
        public IList<SkillInstance> All()
        {
            return SCRIPT_MAP.Values.ToList();
        }
        public SkillInstance Find(string id)
        {
            var skill = default(SkillInstance);
            SCRIPT_MAP.TryGetValue(id, out skill);
            return skill;
        }
        public void Add(SkillInstance skill)
        {
            SCRIPT_MAP.AddOrUpdate(skill.Id, skill, (key, old) => skill);
        }
    }
}