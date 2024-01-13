namespace EventHorizon.Zone.System.Combat.Plugin.Skill.State;

using EventHorizon.Zone.System.Combat.Plugin.Skill.Model;

using global::System.Collections.Concurrent;
using global::System.Collections.Generic;
using global::System.Linq;

public class InMemorySkillRepository : SkillRepository
{
    private readonly ConcurrentDictionary<string, SkillInstance> _map = new();

    public IList<SkillInstance> All()
    {
        return _map.Values.ToList();
    }

    public SkillInstance Find(
        string id
    )
    {
        _map.TryGetValue(
            id,
            out var skill
        );
        return skill;
    }

    public void Set(
        SkillInstance skill
    )
    {
        _map.AddOrUpdate(
            skill.Id,
            skill,
            (_, _) => skill
        );
    }
}
