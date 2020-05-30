namespace EventHorizon.Zone.System.Combat.Plugin.Skill.Load
{
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.Core.Model.Json;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;
    
    public class LoadCombatSystemPluginSkillHandler : IRequestHandler<LoadSystemCombatPluginSkill>
    {
        readonly IMediator _mediator;
        readonly ServerInfo _serverInfo;
        readonly IJsonFileLoader _fileLoader;

        public LoadCombatSystemPluginSkillHandler(
            IMediator mediator,
            ServerInfo serverInfo,
            IJsonFileLoader fileLoader
        )
        {
            _mediator = mediator;
            _serverInfo = serverInfo;
            _fileLoader = fileLoader;
        }

        public async Task<Unit> Handle(
            LoadSystemCombatPluginSkill request, 
            CancellationToken cancellationToken
        )
        {
            // Load Combat Skills
            await _mediator.Publish(
               new LoadCombatSkillsEvent()
            );
            // Load Combat Skill Scripts
            await _mediator.Publish(
               new LoadSystemCombatSkillScriptsEvent()
            );

            return Unit.Value;
        }
    }
}