namespace EventHorizon.Zone.System.Combat.Plugin.Skill.Editor.Model
{
    using global::System.Collections.Generic;

    using EventHorizon.Zone.System.Combat.Plugin.Skill.Model;

    public struct EditorCombatSkills
    {
        public IList<SkillInstance> SkillList { get; }

        public EditorCombatSkills(
            IList<SkillInstance> skillList
        )
        {
            SkillList = skillList;
        }
    }
}
