namespace EventHorizon.Zone.System.Combat.Plugin.Skill.Editor.Model
{
    using global::System.Collections.Generic;

    using EventHorizon.Zone.System.Server.Scripts.Model.Details;

    public struct EditorSystemCombatSkillScriptsFile
    {
        public IEnumerable<ServerScriptDetails> EffectList { get; }
        public IEnumerable<ServerScriptDetails> ValidatorList { get; }

        public EditorSystemCombatSkillScriptsFile(
            IEnumerable<ServerScriptDetails> effectList,
            IEnumerable<ServerScriptDetails> validatorList
        )
        {
            EffectList = effectList;
            ValidatorList = validatorList;
        }
    }
}
