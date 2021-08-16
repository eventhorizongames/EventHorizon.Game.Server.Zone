using System.Collections.Generic;

using EventHorizon.Zone.System.Combat.Plugin.Skill.Model;

namespace EventHorizon.Zone.System.Combat.Plugin.Skill.Editor.Model
{
    public struct EditorCombatSkills
    {
        public IList<SkillInstance> SkillList { get; }

        public EditorCombatSkills(
            IList<SkillInstance> skillList
        )
        {
            this.SkillList = skillList;
        }
    }
}
