using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Model.Id;
using EventHorizon.Zone.System.Combat.Events.Skill.Runner;
using EventHorizon.Zone.System.Player.Plugin.Action.Events.Register;
using MediatR;

namespace EventHorizon.Zone.System.Combat.Skill.Action
{
    public class RegisterCombatSkillPlayerActionsCommandHandler : INotificationHandler<ReadyForPlayerActionRegistration>
    {
        readonly IMediator _mediator;
        readonly IdPool _idPool;

        public RegisterCombatSkillPlayerActionsCommandHandler(
            IMediator mediator, 
            IdPool idPool
        )
        {
            _mediator = mediator;
            _idPool = idPool;
        }

        public async Task Handle(
            ReadyForPlayerActionRegistration notification,
            CancellationToken cancellationToken
        )
        {
            await _mediator.Send(
                new RegisterPlayerAction(
                    _idPool.NextId(),
                    PlayerSkillActions.RUN_SKILL,
                    new RunSkillWithTargetOfEntityEvent()
                )
            );
        }
    }
}