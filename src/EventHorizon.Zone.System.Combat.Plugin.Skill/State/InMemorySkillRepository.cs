namespace EventHorizon.Zone.System.Combat.Plugin.Skill.State
{
    using EventHorizon.Zone.System.Combat.Plugin.Skill.Model;
    using global::System.Collections.Concurrent;
    using global::System.Collections.Generic;
    using global::System.Linq;

    public class InMemorySkillRepository : SkillRepository
    {
        private readonly ConcurrentDictionary<string, SkillInstance> SCRIPT_MAP = new ConcurrentDictionary<string, SkillInstance>();

        public IList<SkillInstance> All()
        {
            return SCRIPT_MAP.Values.ToList();
        }

        public SkillInstance Find(
            string id
        )
        {
            SCRIPT_MAP.TryGetValue(
                id,
                out var skill
            );
            return skill;
        }

        public void Set(
            SkillInstance skill
        )
        {
            SCRIPT_MAP.AddOrUpdate(
                skill.Id,
                skill,
                (_, __) => skill
            );
        }
    }
}