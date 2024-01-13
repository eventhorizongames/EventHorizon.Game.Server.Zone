namespace EventHorizon.Zone.System.Combat.Plugin.Skill.State;

using EventHorizon.Zone.System.Combat.Plugin.Skill.Model;

using global::System.Collections.Generic;

public interface SkillRepository
{
    IList<SkillInstance> All();
    SkillInstance Find(
        string id
    );
    void Set(
        SkillInstance skill
    );
}
