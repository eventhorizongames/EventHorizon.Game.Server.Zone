using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Events.Entity.Find;
using EventHorizon.Game.Server.Zone.External.DateTimeService;
using EventHorizon.Game.Server.Zone.Model.Entity;
using EventHorizon.Plugin.Zone.System.Combat.Skill.Entity.State;
using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Skill.Cooldown
{
    public class SetCooldownOnSkillHandler : INotificationHandler<SetCooldownOnSkillEvent>
    {
        readonly IMediator _mediator;
        readonly IDateTimeService _dateTime;
        public SetCooldownOnSkillHandler(
            IMediator mediator,
            IDateTimeService dateTime
        )
        {
            _mediator = mediator;
            _dateTime = dateTime;
        }
        public async Task Handle(SetCooldownOnSkillEvent notification, CancellationToken cancellationToken)
        {
            var caster = await _mediator.Send(
                new GetEntityByIdEvent
                {
                    EntityId = notification.CasterId
                }
            );
            var skillState = caster.GetProperty<SkillState>(SkillState.PROPERTY_NAME);
            var skill = skillState.SkillList[notification.SkillId];
            skill.CooldownFinishes = _dateTime.Now
            .AddMilliseconds(
                notification.CoolDown
            );
            skillState.SkillList[notification.SkillId] = skill;
        }
    }
}