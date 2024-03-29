namespace EventHorizon.Zone.System.Combat.Skill.Runner;

using EventHorizon.Game.I18n;
using EventHorizon.Game.I18n.Model;
using EventHorizon.Zone.Core.Events.Entity.Find;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Combat.Events.Client.Messsage;
using EventHorizon.Zone.System.Combat.Model.Client.Messsage;
using EventHorizon.Zone.System.Combat.Plugin.Skill.Events.Runner;
using EventHorizon.Zone.System.Combat.Plugin.Skill.Find;
using EventHorizon.Zone.System.Combat.Plugin.Skill.Model;
using EventHorizon.Zone.System.Combat.Plugin.Skill.Model.Entity;
using EventHorizon.Zone.System.Combat.Plugin.Skill.Runner.Effect;
using EventHorizon.Zone.System.Combat.Plugin.Skill.Validation;

using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

using Microsoft.Extensions.Logging;

public class RunSkillWithTargetOfEntityHandler
    : INotificationHandler<RunSkillWithTargetOfEntityEvent>
{
    private readonly ILogger _logger;
    private readonly IMediator _mediator;
    private readonly I18nResolver _i18nResolver;

    public RunSkillWithTargetOfEntityHandler(
        ILogger<RunSkillWithTargetOfEntityHandler> logger,
        IMediator mediator,
        I18nResolver i18nResolver
    )
    {
        _logger = logger;
        _mediator = mediator;
        _i18nResolver = i18nResolver;
    }

    public async Task Handle(
        RunSkillWithTargetOfEntityEvent notification,
        CancellationToken cancellationToken
    )
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
        var targetPosition = notification.TargetPosition;
        if (!caster.IsFound()
            || !target.IsFound()
            || !skill.IsFound())
        {
            _logger.LogError(
                "Exception during casting of Skill. {ConnectionId} {CasterId} {TargetId} {SkillId} {TargetPosition} {@Caster} {@Target} {@Skill}",
                notification.ConnectionId,
                notification.CasterId,
                notification.TargetId,
                notification.SkillId,
                notification.TargetPosition,
                caster,
                target,
                skill
            );
            // Send Message to Connection, Code: skill_exception
            await _mediator.Publish(
                SingleClientActionMessageFromCombatSystemEvent.Create(
                    notification.ConnectionId,
                     new MessageFromCombatSystemData
                     {
                         MessageCode = "skill_exception",
                         Message = _i18nResolver.Resolve(
                            "default",
                            "skillExceptionMessage"
                        )
                     }
                )
            );
            return;
        }
        var skillState = caster.GetProperty<SkillState>(
            SkillState.PROPERTY_NAME
        );

        // Make Sure Caster can cast Skill
        if (CasterDoesNotHaveSkill(
            skillState,
            skill.Id
        ))
        {
            // Send Message to Caster, Code: does_not_have_skill
            await _mediator.Publish(
                SingleClientActionMessageFromCombatSystemEvent.Create(
                    notification.ConnectionId,
                    new MessageFromCombatSystemData
                    {
                        MessageCode = "does_not_have_skill",
                        Message = _i18nResolver.Resolve(
                            "default",
                            "casterDoesNotHaveSkill",
                            new I18nTokenValue
                            {
                                Token = "casterName",
                                Value = caster.Name
                            },
                            new I18nTokenValue
                            {
                                Token = "skillName",
                                Value = skill.Name
                            }
                        )
                    }
                )
            );
            return;
        }

        // Run Validators of Skill
        var validationResponse = await _mediator.Send(
            new RunSkillValidation(
                skill,
                skill.ValidatorList,
                caster,
                target,
                targetPosition
            )
        );
        if (validationResponse.Any(a => !a.Success))
        {
            _logger.LogError(
                "Failed to validate Skill. Response: {@ValidationResponse}",
                validationResponse
            );
            // Run Failed Effects for Skill
            var state = new Dictionary<string, object>(notification.Data ?? new Dictionary<string, object>())
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

            foreach (var failledEffect in skill.FailedList ?? Array.Empty<SkillEffect>())
            {
                await _mediator.Publish(
                    new RunSkillEffectWithTargetOfEntityEvent
                    {
                        ConnectionId = notification.ConnectionId,
                        SkillEffect = failledEffect,
                        Caster = caster,
                        Target = target,
                        Skill = skill,
                        TargetPosition = targetPosition,
                        State = state,
                    }
                );
            }
            return;
        }

        foreach (var skillEffect in skill.Next ?? Array.Empty<SkillEffect>())
        {
            await _mediator.Publish(
                new RunSkillEffectWithTargetOfEntityEvent
                {
                    ConnectionId = notification.ConnectionId,
                    SkillEffect = skillEffect,
                    Caster = caster,
                    Target = target,
                    Skill = skill,
                    TargetPosition = targetPosition,
                    State = notification.Data,
                }
            );
        }
    }

    private bool CasterDoesNotHaveSkill(
        SkillState skillState,
        string skillId
    )
    {
        return !skillState
            .SkillMap
            .Contains(
                skillId
            );
    }
}
