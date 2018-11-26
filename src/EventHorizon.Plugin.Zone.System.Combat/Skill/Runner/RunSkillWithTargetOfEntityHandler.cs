using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Events.Entity.Find;
using EventHorizon.Game.Server.Zone.External.DateTimeService;
using EventHorizon.Game.Server.Zone.Model.Entity;
using EventHorizon.Plugin.Zone.System.Combat.Client;
using EventHorizon.Plugin.Zone.System.Combat.Skill.Cooldown;
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

        public RunSkillWithTargetOfEntityHandler(
            ILogger<RunSkillWithTargetOfEntityHandler> logger,
            IMediator mediator,
            IDateTimeService dateService
        )
        {
            _logger = logger;
            _mediator = mediator;
            _dateService = dateService;
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
                // TODO: Throw error to Caster, Code: invalid_caster_or_target_or_skill
                _logger.LogError(
                    "Skill failed to find caster or target. {casterId} {targetId} {skillId} {@caster} {@target} {@skill}",
                    notification.CasterId,
                    notification.TargetId,
                    notification.SkillId,
                    caster,
                    target,
                    skill
                );
                return;
            }
            var skillState = caster.GetProperty<SkillState>(SkillState.PROPERTY_NAME);

            // Make Sure Caster can cast Skill
            if (CasterDoesNotHaveSkill(skillState, skill.Id))
            {
                // TODO: Throw error to Caster, Code: does_not_have_skill, Data: { skillId: string; }
                return;
            }

            // Validate Skill Cooldown
            if (SkillNotReady(caster, skillState, skill))
            {
                // Throw error to Caster, Code: skill_not_ready
                // TODO: Update Message to use LocalizationService
                await _mediator.Publish(
                    new SingleClientActionMessageToCombatSystemLogEvent
                    {
                        ConnectionId = notification.ConnectionId,
                        Data = new MessageToCombatSystemLogData
                        {
                            MessageCode = "skill_not_ready",
                            Message = $"{skill.Name} is not ready."
                        }
                    }
                ).ConfigureAwait(false);
                return;
            }

            // Run Validators of Skill
            var validationResponse = await ValidateSkill(caster, target, skill);
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
                        "ValidationMessage",
                        validationResponse.ErrorMessage
                    },
                    {
                        "Skill",
                        skill
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
                            State = state
                        }
                    ).ConfigureAwait(false);
                }
                return;
            }

            _logger.LogDebug(
                "Running Skill. {casterId} {targetId} {skillId} {@caster} {@target} {@skill}",
                notification.CasterId,
                notification.TargetId,
                notification.SkillId,
                caster,
                target,
                skill
            );

            foreach (var skillEffect in skill.EffectList)
            {
                await _mediator.Publish(
                    new RunSkillEffectWithTargetOfEntityEvent
                    {
                        ConnectionId = notification.ConnectionId,
                        SkillEffect = skillEffect,
                        Caster = caster,
                        Target = target
                    }
                ).ConfigureAwait(false);
            }

            // Mark Skill for cooldown
            await _mediator.Publish(
                new SetCooldownOnSkillEvent
                {
                    CasterId = caster.Id,
                    SkillId = skill.Id,
                    CoolDown = skill.CoolDown
                }
            ).ConfigureAwait(false);
        }

        private bool CasterDoesNotHaveSkill(SkillState skillState, string skillId)
        {
            return !skillState
                .SkillList
                .ContainsKey(skillId);
        }

        private bool SkillNotReady(
            IObjectEntity caster,
            SkillState skillState,
            SkillInstance skill
        )
        {
            _logger.LogInformation("Date: {date}", skillState
                .SkillList[skill.Id]
                .CooldownFinishes);
            _logger.LogInformation("Date.now: {now}", _dateService.Now);
            _logger.LogInformation("SkillReady: {ready}", _dateService.Now < skillState
                .SkillList[skill.Id]
                .CooldownFinishes);
            return _dateService.Now < skillState
                .SkillList[skill.Id]
                .CooldownFinishes;
        }

        private async Task<SkillValidatorResponse> ValidateSkill(
            IObjectEntity caster,
            IObjectEntity target,
            SkillInstance skill
        )
        {
            // Run Validation scripts of Skill, return validations including errors.
            var validationResponseList = await _mediator.Send(
                new RunValidateForSkillEvent
                {
                    Skill = skill,
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
    }
}