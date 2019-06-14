using System.Collections.Generic;
using EventHorizon.Plugin.Zone.System.Combat.Skill.Model;

namespace EventHorizon.Plugin.Zone.System.Combat.Editor.Model
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