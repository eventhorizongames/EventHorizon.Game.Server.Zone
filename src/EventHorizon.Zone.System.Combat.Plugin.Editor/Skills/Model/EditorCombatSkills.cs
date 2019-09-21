using System.Collections.Generic;
using EventHorizon.Plugin.Zone.System.Combat.Skill.Model;

namespace EventHorizon.Zone.System.Combat.Plugin.Editor.Skills.Model
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