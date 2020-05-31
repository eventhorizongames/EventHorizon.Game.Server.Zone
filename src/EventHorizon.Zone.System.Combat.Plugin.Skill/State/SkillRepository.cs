namespace EventHorizon.Zone.System.Combat.Plugin.Skill.State
{
    using global::System.Collections.Generic;
    using EventHorizon.Zone.System.Combat.Plugin.Skill.Model;

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
}