using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Model.Id;
using EventHorizon.Zone.System.Agent.Plugin.Companion.Events.Skill.Run;
using EventHorizon.Zone.System.Player.Plugin.Action.Events.Register;
using MediatR;

namespace EventHorizon.Zone.System.Agent.Plugin.Companion.Skill.Action
{
    public class RegisterCompanionSkillPlayerActionsCommandHandler : INotificationHandler<ReadyForPlayerActionRegistration>
    {
        readonly IMediator _mediator;
        readonly IdPool _idPool;

        public RegisterCompanionSkillPlayerActionsCommandHandler(
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
                    PlayerCompanionSkillActions.RUN_SKILL_ON_COMPANION,
                    new RunPlayerCompanionSkillEvent()
                )
            );
        }
    }
}