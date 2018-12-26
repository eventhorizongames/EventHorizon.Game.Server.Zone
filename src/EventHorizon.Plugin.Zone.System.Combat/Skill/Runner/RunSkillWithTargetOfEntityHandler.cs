using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.I18n;
using EventHorizon.Game.I18n.Model;
using EventHorizon.Game.Server.Zone.Events.Entity.Find;
using EventHorizon.Game.Server.Zone.External.DateTimeService;
using EventHorizon.Game.Server.Zone.Model.Entity;
using EventHorizon.Plugin.Zone.System.Combat.Client.Messsage;
using EventHorizon.Plugin.Zone.System.Combat.Skill.Entity.State;
using EventHorizon.Plugin.Zone.System.Combat.Skill.Find;
using EventHorizon.Plugin.Zone.System.Combat.Skill.Model;
using EventHorizon.Plugin.Zone.System.Combat.Skill.Runner.EffectRunner;
using EventHorizon.Plugin.Zone.System.Combat.Skill.State;
using EventHorizon.Plugin.Zone.System.Combat.Skill.Validation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventHorizon.Plugin.Zone.System.Combat.Skill.Runner
{
    public class RunSkillWithTargetOfEntityHandler : INotificationHandler<RunSkillWithTargetOfEntityEvent>
    {
        readonly ILogger _logger;
        readonly IMediator _mediator;
        readonly IDateTimeService _dateService;
        readonly I18nResolver _i18nResolver;

        public RunSkillWithTargetOfEntityHandler(
            ILogger<RunSkillWithTargetOfEntityHandler> logger,
            IMediator mediator,
            IDateTimeService dateService,
            I18nResolver i18nResolver
        )
        {
            _logger = logger;
            _mediator = mediator;
            _dateService = dateService;
            _i18nResolver = i18nResolver;
        }
        public async Task Handle(RunSkillWithTargetOfEntityEvent notification, CancellationToken cancellationToken)
        {
            var caster = await _mediator.Send(
                new GetEntityByIdEvent
                {
                    EntityId = notification.CasterId
                }
            );
            var target = await _mediator.Send(
                new GetEntityByIdEvent
                {
                    EntityId = notification.TargetId
                }
            );
            var skill = await _mediator.Send(
                new FindSkillByIdEvent
                {
                    SkillId = notification.SkillId
                }
            );
            if (!caster.IsFound()
                || !target.IsFound()
                || !skill.IsFound())
            {
                _logger.LogError(
                    "Exception during casting of Skill. {connectionId} {casterId} {targetId} {skillId} {@caster} {@target} {@skill}",
                    notification.ConnectionId,
                    notification.CasterId,
                    notification.TargetId,
                    notification.SkillId,
                    caster,
                    target,
                    skill
                );
                // Send Message to Connection, Code: skill_exception
                await _mediator.Publish(
                    new SingleClientActionMessageFromCombatSystemEvent
                    {
                        ConnectionId = notification.ConnectionId,
                        Data = new MessageFromCombatSystemData
                        {
                            MessageCode = "skill_exception",
                            Message = _i18nResolver.Resolve(
                                "default",
                                "SkillExceptionMessage"
                            )
                        }
                    }
                ).ConfigureAwait(false);
                return;
            }
            var skillState = caster.GetProperty<SkillState>(SkillState.PROPERTY_NAME);

            // Make Sure Caster can cast Skill
            if (CasterDoesNotHaveSkill(skillState, skill.Id))
            {
                // Send Message to Caster, Code: does_not_have_skill
                await _mediator.Publish(
                    new SingleClientActionMessageFromCombatSystemEvent
                    {
                        ConnectionId = notification.ConnectionId,
                        Data = new MessageFromCombatSystemData
                        {
                            MessageCode = "does_not_have_skill",
                            Message = _i18nResolver.Resolve(
                                "default",
                                "CasterDoesNotHaveSkill",
                                new I18nTokenValue
                                {
                                    Token = "casterName",
                                    Value = caster.Id.ToString()
                                },
                                new I18nTokenValue
                                {
                                    Token = "skillName",
                                    Value = skill.Name
                                }
                            )
                        }
                    }
                ).ConfigureAwait(false);
                return;
            }

            // Run Validators of Skill
            var validationResponse = await _mediator.Send(
                new RunValidateForSkillEvent
                {
                    Caster = caster,
                    Target = target,
                    Skill = skill,
                }
            );
            if (!validationResponse.Success)
            {
                _logger.LogError(
                    "Failed to validate Skill. Response: {@ValidationResponse}",
                    validationResponse
                );
                // Run Failed Effects for Skill
                var state = new Dictionary<string, object>
                {
                    {
                        "Code",
                        "skill_effect_validation_failed"
                    },
                    {
                        "ValidationResponse",
                        validationResponse
                    }
                };

                foreach (var failledEffect in skill.FailedList ?? new SkillEffect[0])
                {
                    await _mediator.Publish(
                        new RunSkillEffectWithTargetOfEntityEvent
                        {
                            ConnectionId = notification.ConnectionId,
                            SkillEffect = failledEffect,
                            Caster = caster,
                            Target = target,
                            Skill = skill,
                            State = state
                        }
                    ).ConfigureAwait(false);
                }
                return;
            }

            foreach (var skillEffect in skill.EffectList)
            {
                await _mediator.Publish(
                    new RunSkillEffectWithTargetOfEntityEvent
                    {
                        ConnectionId = notification.ConnectionId,
                        SkillEffect = skillEffect,
                        Caster = caster,
                        Target = target,
                        Skill = skill
                    }
                ).ConfigureAwait(false);
            }
        }

        private bool CasterDoesNotHaveSkill(SkillState skillState, string skillId)
        {
            return !skillState
                .SkillList
                .ContainsKey(skillId);
        }
    }
}