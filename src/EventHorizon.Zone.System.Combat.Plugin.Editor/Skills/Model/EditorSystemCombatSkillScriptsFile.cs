using System.Collections.Generic;
using EventHorizon.Zone.System.Server.Scripts.Model.Details;

namespace EventHorizon.Zone.System.Combat.Plugin.Editor.Skills.Model
{
    public struct EditorSystemCombatSkillScriptsFile
    {
        public IEnumerable<ServerScriptDetails> EffectList { get; }
        public IEnumerable<ServerScriptDetails> ValidatorList { get; }

        public EditorSystemCombatSkillScriptsFile(
            IEnumerable<ServerScriptDetails> effectList,
            IEnumerable<ServerScriptDetails> validatorList
        )
        {
            this.EffectList = effectList;
            this.ValidatorList = validatorList;
        }
    }
}