using System.Collections.Generic;
using EventHorizon.Plugin.Zone.System.Combat.Skill.Model;

namespace EventHorizon.Plugin.Zone.System.Combat.Editor.Model
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