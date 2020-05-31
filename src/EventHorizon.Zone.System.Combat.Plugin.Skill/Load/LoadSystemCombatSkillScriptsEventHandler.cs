namespace EventHorizon.Zone.System.Combat.Plugin.Skill.Load
{
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;

    public class LoadSystemCombatSkillScriptsEventHandler 
        : INotificationHandler<LoadSystemCombatSkillScriptsEvent>
    {
        private readonly IMediator _mediator;

        public LoadSystemCombatSkillScriptsEventHandler(
            IMediator mediator
        )
        {
            _mediator = mediator;
        }

        public async Task Handle(
            LoadSystemCombatSkillScriptsEvent notification,
            CancellationToken cancellationToken
        )
        {
            await _mediator.Send(
                new LoadCombatSkillEffectScripts()
            );
            await _mediator.Send(
                new LoadCombatSkillValidatorScripts()
            );
        }
    }
}