using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Model.Entity;
using EventHorizon.Plugin.Zone.System.Combat.Skill.ClientAction;
using EventHorizon.Plugin.Zone.System.Combat.Skill.Model;
using EventHorizon.Plugin.Zone.System.Combat.Skill.State;

namespace EventHorizon.Plugin.Zone.System.Combat.Skill.Runner
{
    public class SkillRunner
    {
        readonly SkillEffectScriptRepository _skillEffectScriptRepository;
        public SkillRunner(
            SkillEffectScriptRepository skillEffectScriptRepository
        )
        {
            _skillEffectScriptRepository = skillEffectScriptRepository;
        }

        public async Task<List<ClientSkillActionEvent>> Run(SkillInstance skill, IObjectEntity caster, IObjectEntity target)
        {
            // TODO: Validate Target; If skill is expecting an Entity or a Position
            // TODO: Validate Type; Active | Passive
            // TODO: Validate the caster is not in CoolDown of this skill
            // Loop through skill effects and create Client Event List
            var clientEventList = new List<ClientSkillActionEvent>();

            await AddToClientEventList(
                clientEventList,
                skill.EffectList,
                caster,
                target
            );

            return clientEventList;
        }

        private async Task AddToClientEventList(List<ClientSkillActionEvent> clientEventList, SkillEffect[] effectList, IObjectEntity caster, IObjectEntity target)
        {
            foreach (var effect in effectList ?? new SkillEffect[0])
            {
                // TODO: Run Validator List
                // TODO: Run Modifier List
                var effectClientEventList =
                    await FindScript(
                        effect.Effect
                    ).Run(
                        caster,
                        target,
                        effect.Data
                    );
                if (effectClientEventList.Count == 0)
                {
                    await AddToClientEventList(
                        clientEventList,
                        effect.Next,
                        caster,
                        target
                    );
                }
                else
                {
                    var effectClientEvent = effectClientEventList[0];
                    effectClientEvent.Next = new List<ClientSkillActionEvent>();
                    await AddToClientEventList(
                        effectClientEvent.Next,
                        effect.Next,
                        caster,
                        target
                    );
                    effectClientEventList[0] = effectClientEvent;
                }
                clientEventList.AddRange(
                    effectClientEventList
                );
            }
        }

        private SkillEffectScript FindScript(string effect)
        {
            var effectScript = _skillEffectScriptRepository.Find(effect);
            if (effectScript.Equals(default(SkillEffectScript)))
            {
                throw new KeyNotFoundException(effect);
            }
            return effectScript;
        }
    }
}