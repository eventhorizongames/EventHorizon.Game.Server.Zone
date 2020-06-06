namespace EventHorizon.Zone.System.Combat.Plugin.Skill.Runner.Effect
{
    using EventHorizon.Zone.Core.Events.ServerAction;
    using EventHorizon.Zone.Core.Model.DateTimeService;
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.System.Combat.Plugin.Skill.ClientAction;
    using EventHorizon.Zone.System.Combat.Plugin.Skill.Model;
    using EventHorizon.Zone.System.Combat.Plugin.Skill.Validation;
    using EventHorizon.Zone.System.Server.Scripts.Events.Run;
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Numerics;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public class RunSkillEffectWithTargetOfEntityEventHandler
        : INotificationHandler<RunSkillEffectWithTargetOfEntityEvent>
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly IDateTimeService _dateTime;

        public RunSkillEffectWithTargetOfEntityEventHandler(
            ILogger<RunSkillEffectWithTargetOfEntityEventHandler> logger,
            IMediator mediator,
            IDateTimeService dateTime
        )
        {
            _logger = logger;
            _mediator = mediator;
            _dateTime = dateTime;
        }

        public async Task Handle(
            RunSkillEffectWithTargetOfEntityEvent notification,
            CancellationToken cancellationToken
        )
        {
            var connectionId = notification.ConnectionId;
            var effect = notification.SkillEffect;
            var caster = notification.Caster;
            var target = notification.Target;
            var skill = notification.Skill;
            var targetPosition = notification.TargetPosition;

            // Run Validators of Skill
            var validationResponse = await RunValidationScripts(
                skill,
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

                foreach (var failledEffect in effect.FailedList ?? Array.Empty<SkillEffect>())
                {
                    await _mediator.Publish(
                        new RunSkillEffectWithTargetOfEntityEvent
                        {
                            ConnectionId = connectionId,
                            SkillEffect = failledEffect,
                            Caster = caster,
                            Target = target,
                            Skill = skill,
                            TargetPosition = targetPosition,
                            State = failedState
                        }
                    );
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
            foreach (var nextEffect in effect.Next ?? Array.Empty<SkillEffect>())
            {
                // Future event based on duration of the current effect.
                await _mediator.Publish(
                    new AddServerActionEvent(
                        _dateTime.Now.AddMilliseconds(
                            effect.Duration
                        ),
                        new RunSkillEffectWithTargetOfEntityEvent
                        {
                            ConnectionId = connectionId,
                            SkillEffect = nextEffect,
                            Caster = caster,
                            Target = target,
                            Skill = skill,
                            State = state
                        }
                    )
                );
            }
        }

        private async Task<SkillValidatorResponse> RunValidationScripts(
            SkillInstance skill,
            SkillEffect effect,
            IObjectEntity caster,
            IObjectEntity target,
            Vector3 targetPosition
        )
        {
            // Run Validation scripts of Skill, return validations including errors.
            var validationResponseList = await _mediator.Send(
                new RunSkillValidation(
                    skill,
                    effect.ValidatorList,
                    caster,
                    target,
                    targetPosition
                )
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
            var effectResponse = (SkillEffectScriptResponse)await _mediator.Send(
                new RunServerScriptCommand(
                    effect.Effect,
                    new Dictionary<string, object>()
                    {
                        { "Caster", caster },
                        { "Target", target },
                        { "Skill", skill },
                        { "TargetPosition", targetPosition },
                        { "EffectData", effect.Data },
                        { "PriorState", state },
                    }
                )
            );

            // Run Client Action events
            foreach (var clientAction in effectResponse.ActionList)
            {
                if (clientAction is ClientSkillActionEvent clientEvent)
                {
                    await _mediator.Publish(
                        ClientActionRunSkillActionEvent.Create(
                            clientEvent
                        )
                    );
                }
                else if (clientAction is ClientActionRunSkillActionForConnectionEvent clientEventToSingle)
                {
                    await _mediator.Publish(
                        ClientActionRunSkillActionToSingleEvent.Create(
                            clientEventToSingle
                        )
                    );
                }
            }
            return effectResponse.State;
        }
    }
}