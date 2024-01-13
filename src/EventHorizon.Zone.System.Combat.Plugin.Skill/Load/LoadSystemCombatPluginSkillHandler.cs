namespace EventHorizon.Zone.System.Combat.Plugin.Skill.Load
{
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;

    public class LoadSystemCombatPluginSkillHandler : IRequestHandler<LoadSystemCombatPluginSkill>
    {
        private readonly IMediator _mediator;

        public LoadSystemCombatPluginSkillHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Handle(
            LoadSystemCombatPluginSkill request,
            CancellationToken cancellationToken
        )
        {
            // Load Combat Skills
            await _mediator.Publish(new LoadCombatSkillsEvent(), cancellationToken);
        }
    }
}
