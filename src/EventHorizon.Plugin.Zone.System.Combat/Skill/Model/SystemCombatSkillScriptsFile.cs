using System.Collections.Generic;

namespace EventHorizon.Plugin.Zone.System.Combat.Skill.Model
{
    public struct SystemCombatSkillScriptsFile
    {
        public List<SkillEffectScript> EffectList { get; set; }
        public List<SkillActionScript> ActionList { get; set; }
        public List<SkillValidatorScript> ValidatorList { get; set; }
    }
}