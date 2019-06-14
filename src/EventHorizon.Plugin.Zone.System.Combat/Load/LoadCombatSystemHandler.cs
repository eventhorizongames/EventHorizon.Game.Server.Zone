using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.External.Info;
using EventHorizon.Game.Server.Zone.External.Json;
using EventHorizon.Plugin.Zone.System.Combat.Particle.Event;
using EventHorizon.Plugin.Zone.System.Combat.Skill.Load;
using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Load
{
    public struct LoadCombatSystemHandler : INotificationHandler<LoadCombatSystemEvent>
    {
        readonly IMediator _mediator;
        readonly ServerInfo _serverInfo;
        readonly IJsonFileLoader _fileLoader;

        public LoadCombatSystemHandler(
            IMediator mediator,
            ServerInfo serverInfo,
            IJsonFileLoader fileLoader
        )
        {
            _mediator = mediator;
            _serverInfo = serverInfo;
            _fileLoader = fileLoader;
        }

        public async Task Handle(LoadCombatSystemEvent notification, CancellationToken cancellationToken)
        {
            // Load Combat Skills
            await _mediator.Publish(
                new LoadCombatSkillsEvent()
            );
            // Load Combat Skill Scripts
            await _mediator.Publish(
                new LoadSystemCombatSkillScriptsEvent()
            );
            // Load Particles
            await _mediator.Publish(
                new LoadCombatParticleSystemEvent()
            );
        }
    }
}