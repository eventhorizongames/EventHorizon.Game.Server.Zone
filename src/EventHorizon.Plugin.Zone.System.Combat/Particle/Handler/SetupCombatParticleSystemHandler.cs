using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Events.Particle.Add;
using EventHorizon.Game.Server.Zone.External.Info;
using EventHorizon.Game.Server.Zone.External.Json;
using EventHorizon.Plugin.Zone.System.Combat.Particle.Event;
using EventHorizon.Plugin.Zone.System.Combat.Particle.Model;
using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Particle.Handler
{
    public class SetupCombatParticleSystemHandler : INotificationHandler<SetupCombatParticleSystemEvent>
    {
        readonly IMediator _mediator;
        readonly IJsonFileLoader _fileLoader;
        readonly ServerInfo _serverInfo;
        public SetupCombatParticleSystemHandler(IMediator mediator, IJsonFileLoader fileLoader, ServerInfo serverInfo)
        {
            _mediator = mediator;
            _fileLoader = fileLoader;
            _serverInfo = serverInfo;
        }
        public async Task Handle(SetupCombatParticleSystemEvent notification, CancellationToken cancellationToken)
        {
            var filePath = Path.Combine(_serverInfo.PluginsPath, "Particle.System.Combat.json");
            var templateFile = await _fileLoader.GetFile<CombatSystemParticleTemplateList>(filePath);

            foreach (var template in templateFile.TemplateList)
            {
                await _mediator.Publish(
                    new AddParticleTemplateEvent
                    {
                        Id = template.Id,
                        Template = template
                    }
                );
            }
        }
    }
}