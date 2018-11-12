using System;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Events.Entity.Find;
using EventHorizon.Game.Server.Zone.External.DateTimeService;
using EventHorizon.Game.Server.Zone.Model.Entity;
using EventHorizon.Plugin.Zone.System.Combat.Skill.Cooldown;
using EventHorizon.Plugin.Zone.System.Combat.Skill.Entity.State;
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
        readonly ISkillRepository _skillRepository;
        readonly IDateTimeService _dateService;

        public RunSkillWithTargetOfEntityHandler(
            ILogger<RunSkillWithTargetOfEntityHandler> logger,
            IMediator mediator,
            ISkillRepository skillRepository,
            IDateTimeService dateService
        )
        {
            _logger = logger;
            _mediator = mediator;
            _skillRepository = skillRepository;
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
            // TODO: Create FindSkillByIdEvent
            var skill = _skillRepository.Find(
                notification.SkillId
            );
            if (!caster.IsFound() || !target.IsFound())
            {
                // TODO: Throw error to Caster, Code: invalid_caster_or_target
                _logger.LogError("Skill failed to find caster or target. {casterId} {targetId} {skillId} {@caster} {@target} {@skill}", notification.CasterId, notification.TargetId, notification.SkillId, caster, target, skill);
                return;
            }
            var skillState = caster.GetProperty<SkillState>(SkillState.PROPERTY_NAME);

            // Make Sure Caster can cast Skill
            if (CasterDoesNotHaveSkill(skillState, skill.Id))
            {
                // TODO: Throw error to Caster, Code: does_not_have_skill, Data: { skillId: string; }
                return;
            }

            // TODO: Validate Skill Cooldown
            if (SkillNotReady(caster, skillState, skill))
            {
                // TODO: Throw error to Caster, Code: skill_not_ready, Data: { skillId: string; }
                return;
            }

            // TODO: Run Validators on Skill
            var validationResponse = await ValidateSkill(caster, target, skill);
            if (!validationResponse.Success)
            {
                _logger.LogError("Failed to validate Skill. Response: {@ValidationResponse}", validationResponse);
                // TODO: Throw error to Caster, Code: skill_validation_failed, Data: { skillId: string; }
                return;
            }

            _logger.LogDebug("Running Skill. {casterId} {targetId} {skillId} {@caster} {@target} {@skill}", notification.CasterId, notification.TargetId, notification.SkillId, caster, target, skill);

            foreach (var skillEffect in skill.EffectList)
            {
                await _mediator.Publish(
                    new RunSkillEffectWithTargetOfEntityEvent
                    {
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
                    Caster = caster.Id,
                    SkillId = skill.Id
                }
            ).ConfigureAwait(false);
        }

        private bool CasterDoesNotHaveSkill(SkillState skillState, string skillId)
        {
            return !skillState
                .SkillList
                .ContainsKey(skillId);
        }

        private bool SkillNotReady(IObjectEntity caster, SkillState skillState, SkillInstance skill)
        {
            var casterSkill = skillState
                .SkillList[skill.Id];
            return _dateService.Now < casterSkill.CooldownFinishes;
        }

        private async Task<SkillValidatorResponse> ValidateSkill(
            IObjectEntity caster,
            IObjectEntity target,
            SkillInstance skill
        )
        {
            // TODO: Run Validation scripts of Skill, return any error validations.
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