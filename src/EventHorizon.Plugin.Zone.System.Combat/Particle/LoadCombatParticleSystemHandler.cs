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
    public class LoadCombatParticleSystemHandler : INotificationHandler<LoadCombatParticleSystemEvent>
    {
        readonly IMediator _mediator;
        readonly IJsonFileLoader _fileLoader;
        readonly ServerInfo _serverInfo;
        public LoadCombatParticleSystemHandler(IMediator mediator, IJsonFileLoader fileLoader, ServerInfo serverInfo)
        {
            _mediator = mediator;
            _fileLoader = fileLoader;
            _serverInfo = serverInfo;
        }
        public async Task Handle(LoadCombatParticleSystemEvent notification, CancellationToken cancellationToken)
        {
            var filePath = Path.Combine(
                _serverInfo.AssetsPath,
                "Particle",
                notification.FileName
            );
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