namespace EventHorizon.Zone.System.Combat.Plugin.Skill.Action
{
    using EventHorizon.Zone.Core.Model.Id;
    using EventHorizon.Zone.System.Combat.Plugin.Skill.Events.Runner;
    using EventHorizon.Zone.System.Player.Plugin.Action.Events.Register;

    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    public class RegisterCombatSkillPlayerActionsCommandHandler
        : INotificationHandler<ReadyForPlayerActionRegistration>
    {
        private readonly IMediator _mediator;
        private readonly IdPool _idPool;

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
