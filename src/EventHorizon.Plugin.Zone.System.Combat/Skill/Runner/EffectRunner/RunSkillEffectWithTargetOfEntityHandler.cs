using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Events.ServerAction;
using EventHorizon.Game.Server.Zone.External.DateTimeService;
using EventHorizon.Game.Server.Zone.Model.Entity;
using EventHorizon.Plugin.Zone.System.Combat.Skill.ClientAction;
using EventHorizon.Plugin.Zone.System.Combat.Skill.Model;
using EventHorizon.Plugin.Zone.System.Combat.Skill.State;
using EventHorizon.Plugin.Zone.System.Combat.Skill.Validation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventHorizon.Plugin.Zone.System.Combat.Skill.Runner.EffectRunner
{
    public class RunSkillEffectWithTargetOfEntityHandler : INotificationHandler<RunSkillEffectWithTargetOfEntityEvent>
    {
        readonly ILogger _logger;
        readonly IMediator _mediator;
        readonly IDateTimeService _dateTime;
        readonly ISkillEffectScriptRepository _skillEffectScriptRepository;
        public RunSkillEffectWithTargetOfEntityHandler(
            ILogger<RunSkillEffectWithTargetOfEntityHandler> logger,
            IMediator mediator,
            IDateTimeService dateTime,
            ISkillEffectScriptRepository skillEffectScriptRepository
        )
        {
            _logger = logger;
            _mediator = mediator;
            _dateTime = dateTime;
            _skillEffectScriptRepository = skillEffectScriptRepository;
        }

        public async Task Handle(RunSkillEffectWithTargetOfEntityEvent notification, CancellationToken cancellationToken)
        {
            var effect = notification.SkillEffect;
            var caster = notification.Caster;
            var target = notification.Target;

            // Run Validators of Skill
            var validationResponse = await RunValidationScripts(
                 effect,
                 caster,
                 target
            );
            if (!validationResponse.Success)
            {
                // TODO: Throw error to Caster, Code: skill_validation_failed, Data: { skillId: string; }
                _logger.LogError(
                    "Failed to validate Skill. Response: {@ValidationResponse}",
                    validationResponse
                );
                return;
            }

            // Run Effect Script
            await RunEffectScript(
                effect,
                caster,
                target
            );

            // Queue up next effects.
            foreach (var nextEffect in effect.Next ?? new SkillEffect[0])
            {
                // Future event based on duration of the current effect.
                await _mediator.Publish(
                    new AddServerActionEvent(
                        _dateTime.Now.AddMilliseconds(
                            effect.Duration
                        ), new RunSkillEffectWithTargetOfEntityEvent
                        {
                            SkillEffect = nextEffect,
                            Caster = caster,
                            Target = target
                        })
                );
            }
        }

        private async Task<SkillValidatorResponse> RunValidationScripts(
            SkillEffect effect,
            IObjectEntity caster,
            IObjectEntity target
        )
        {
            // Run Validation scripts of Skill, return validations including errors.
            var validationResponseList = await _mediator.Send(
                new RunValidateForSkillEffectEvent
                {
                    SkillEffect = effect,
                    Caster = caster,
                    Target = target
                }
            );
            foreach (var validationResponse in validationResponseList)
            {
                if (!validationResponse.Success)
                {
                    return validationResponse;
                }
            }

            return new SkillValidatorResponse
            {
                Success = true
            };
        }

        private async Task RunEffectScript(SkillEffect effect, IObjectEntity caster, IObjectEntity target)
        {
            var effectClientEventList =
                await FindScript(
                    effect.Effect
                ).Run(
                    _mediator,
                    caster,
                    target,
                    effect.Data
                );

            // Run Client Action events
            foreach (var clientEvent in effectClientEventList)
            {
                await _mediator.Publish(
                    new ClientActionRunSkillActionEvent
                    {
                        Data = clientEvent
                    }
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