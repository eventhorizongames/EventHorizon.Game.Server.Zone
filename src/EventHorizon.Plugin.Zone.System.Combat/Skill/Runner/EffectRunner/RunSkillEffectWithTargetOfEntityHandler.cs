using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Events.ServerAction;
using EventHorizon.Game.Server.Zone.External.DateTimeService;
using EventHorizon.Game.Server.Zone.Model.Entity;
using EventHorizon.Plugin.Zone.System.Combat.Client;
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
                _logger.LogError(
                    "Failed to validate Skill. Response: {@ValidationResponse}",
                    validationResponse
                );
                // Send error Log to Caster, Code: skill_effect_validation_failed, Data: { skillId: string; }
                await _mediator.Publish(
                    new SingleClientActionMessageToCombatSystemLogEvent
                    {
                        ConnectionId = notification.ConnectionId,
                        Data = new MessageToCombatSystemLogData
                        {
                            Message = "Error validating Skill",
                            Data = new Dictionary<string, string>
                            {
                                {
                                    "code",
                                    "skill_effect_validation_failed"
                                },
                                {
                                    "validationMessage",
                                    validationResponse.ErrorMessage
                                },
                                {
                                    "skillId",
                                    notification.SkillEffect.Effect
                                },
                                {
                                    "casterId",
                                    notification.Caster.Id.ToString()
                                },
                                {
                                    "targetId",
                                    notification.Target.Id.ToString()
                                }
                            }
                        }
                    }
                ).ConfigureAwait(false);
                return;
            }

            // Run Effect Script
            var state = await RunEffectScript(
                effect,
                caster,
                target,
                notification.State
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
                            ConnectionId = notification.ConnectionId,
                            SkillEffect = nextEffect,
                            Caster = caster,
                            Target = target,
                            State = state
                        })
                ).ConfigureAwait(false);
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

        private async Task<IDictionary<string, object>> RunEffectScript(SkillEffect effect, IObjectEntity caster, IObjectEntity target, IDictionary<string, object> state)
        {
            var effectResponse =
                await FindScript(
                    effect.Effect
                ).Run(
                    _mediator,
                    caster,
                    target,
                    effect.Data,
                    state
                );

            // Run Client Action events
            foreach (var clientEvent in effectResponse.ActionList)
            {
                await _mediator.Publish(
                    new ClientActionRunSkillActionEvent
                    {
                        Data = clientEvent
                    }
                ).ConfigureAwait(false);
            }
            return effectResponse.State;
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