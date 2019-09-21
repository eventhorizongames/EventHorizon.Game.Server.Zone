using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.ServerAction;
using EventHorizon.Zone.Core.Model.DateTimeService;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Combat.Client.Messsage;
using EventHorizon.Zone.System.Combat.Skill.ClientAction;
using EventHorizon.Zone.System.Combat.Skill.Model;
using EventHorizon.Zone.System.Combat.Skill.State;
using EventHorizon.Zone.System.Combat.Script;
using EventHorizon.Zone.System.Combat.Skill.Validation;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Numerics;

namespace EventHorizon.Zone.System.Combat.Skill.Runner.EffectRunner
{
    public class RunSkillEffectWithTargetOfEntityHandler : INotificationHandler<RunSkillEffectWithTargetOfEntityEvent>
    {
        readonly ILogger _logger;
        readonly IMediator _mediator;
        readonly IScriptServices _scriptServices;
        readonly IDateTimeService _dateTime;
        readonly ISkillEffectScriptRepository _skillEffectScriptRepository;
        public RunSkillEffectWithTargetOfEntityHandler(
            ILogger<RunSkillEffectWithTargetOfEntityHandler> logger,
            IMediator mediator,
            IScriptServices scriptServices,
            IDateTimeService dateTime,
            ISkillEffectScriptRepository skillEffectScriptRepository
        )
        {
            _logger = logger;
            _mediator = mediator;
            _scriptServices = scriptServices;
            _dateTime = dateTime;
            _skillEffectScriptRepository = skillEffectScriptRepository;
        }

        public async Task Handle(RunSkillEffectWithTargetOfEntityEvent notification, CancellationToken cancellationToken)
        {
            var connectionId = notification.ConnectionId;
            var effect = notification.SkillEffect;
            var caster = notification.Caster;
            var target = notification.Target;
            var skill = notification.Skill;
            var targetPosition = notification.TargetPosition;

            // Run Validators of Skill
            var validationResponse = await RunValidationScripts(
                 effect,
                 caster,
                 target,
                 targetPosition
            );
            if (!validationResponse.Success)
            {
                _logger.LogError(
                    "Failed to validate Skill. Response: {@ValidationResponse}",
                    validationResponse
                );
                // Run Failed Effects for Effect
                var failedState = new Dictionary<string, object>
                {
                    {
                        "Code",
                        "effect_validation_failed"
                    },
                    {
                        "ValidationResponse",
                        validationResponse
                    },
                    {
                        "Effect",
                        effect
                    }
                };

                foreach (var failledEffect in effect.FailedList ?? new SkillEffect[0])
                {
                    await _mediator.Publish(
                        new RunSkillEffectWithTargetOfEntityEvent
                        {
                            ConnectionId = connectionId,
                            SkillEffect = failledEffect,
                            Caster = caster,
                            Target = target,
                            Skill = skill,
                            State = failedState
                        }
                    ).ConfigureAwait(false);
                }
                return;
            }

            // Run Effect Script
            var state = await RunEffectScript(
                effect,
                caster,
                target,
                skill,
                targetPosition,
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
                            ConnectionId = connectionId,
                            SkillEffect = nextEffect,
                            Caster = caster,
                            Target = target,
                            Skill = skill,
                            State = state
                        })
                ).ConfigureAwait(false);
            }
        }

        private async Task<SkillValidatorResponse> RunValidationScripts(
            SkillEffect effect,
            IObjectEntity caster,
            IObjectEntity target,
            Vector3 targetPosition
        )
        {
            // Run Validation scripts of Skill, return validations including errors.
            var validationResponseList = await _mediator.Send(
                new RunValidateForSkillEffectEvent
                {
                    SkillEffect = effect,
                    Caster = caster,
                    Target = target,
                    TargetPosition = targetPosition
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

        private async Task<IDictionary<string, object>> RunEffectScript(
            SkillEffect effect,
            IObjectEntity caster,
            IObjectEntity target,
            SkillInstance skill,
            Vector3 targetPosition,
            IDictionary<string, object> state
        )
        {
            var effectResponse =
                await FindScript(
                    effect.Effect
                ).Run(
                    _scriptServices,
                    caster,
                    target,
                    skill,
                    targetPosition,
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