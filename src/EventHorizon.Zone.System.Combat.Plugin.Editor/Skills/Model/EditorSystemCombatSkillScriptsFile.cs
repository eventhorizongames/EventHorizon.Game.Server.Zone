using System.Collections.Generic;
using EventHorizon.Zone.System.Combat.Skill.Model;

namespace EventHorizon.Zone.System.Combat.Plugin.Editor.Skills.Model
{
    public struct EditorSystemCombatSkillScriptsFile
    {
        public IEnumerable<SkillEffectScript> EffectList { get; }
        public IEnumerable<SkillValidatorScript> ValidatorList { get; }

        public EditorSystemCombatSkillScriptsFile(
            IEnumerable<SkillEffectScript> effectList,
            IEnumerable<SkillValidatorScript> validatorList
        )
        {
            this.EffectList = effectList;
            this.ValidatorList = validatorList;
        }
    }
}