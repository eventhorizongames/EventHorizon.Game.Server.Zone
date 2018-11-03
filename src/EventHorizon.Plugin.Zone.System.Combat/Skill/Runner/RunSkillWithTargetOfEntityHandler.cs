using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Events.Entity.Find;
using EventHorizon.Plugin.Zone.System.Combat.Skill.Runner.EffectRunner;
using EventHorizon.Plugin.Zone.System.Combat.Skill.State;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventHorizon.Plugin.Zone.System.Combat.Skill.Runner
{
    public class RunSkillWithTargetOfEntityHandler : INotificationHandler<RunSkillWithTargetOfEntityEvent>
    {
        readonly ILogger _logger;
        readonly IMediator _mediator;
        readonly ISkillRepository _skillRepository;
        public RunSkillWithTargetOfEntityHandler(
            ILogger<RunSkillWithTargetOfEntityHandler> logger,
            IMediator mediator,
            ISkillRepository skillRepository
        )
        {
            _logger = logger;
            _mediator = mediator;
            _skillRepository = skillRepository;
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
                _logger.LogError("Skill failed to find caster or target. {casterId} {targetId} {skillId} {@caster} {@target} {@skill}", notification.CasterId, notification.TargetId, notification.SkillId, caster, target, skill);
                return;
            }

            // TODO: Validate Cooldown on Caster

            _logger.LogInformation("Running Skill. {casterId} {targetId} {skillId} {@caster} {@target} {@skill}", notification.CasterId, notification.TargetId, notification.SkillId, caster, target, skill);

            foreach (var skillEffect in skill.EffectList)
            {
                await _mediator.Publish(
                    new RunSkillEffectWithTargetOfEntityEvent
                    {
                        SkillEffect = skillEffect,
                            Caster = caster,
                            Target = target
                    }
                );
            }
            
            // TODO: Mark Skill for cooldown
        }
    }
}