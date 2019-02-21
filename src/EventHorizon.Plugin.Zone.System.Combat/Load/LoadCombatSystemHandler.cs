using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.External.Info;
using EventHorizon.Game.Server.Zone.External.Json;
using EventHorizon.Plugin.Zone.System.Combat.Events;
using EventHorizon.Plugin.Zone.System.Combat.Gui;
using EventHorizon.Plugin.Zone.System.Combat.Load.Model;
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
            var filePath = Path.Combine(_serverInfo.SystemsPath, "System.Combat.json");
            var combatSystemConfigurationFile = await _fileLoader.GetFile<CombatSystemFile>(filePath);

            // Load Combat Skills
            await _mediator.Publish(
                new LoadCombatSkillsEvent(
                    combatSystemConfigurationFile.Skills.File
                )
            );
            // Load Combat Skill Scripts
            await _mediator.Publish(
                new LoadSystemCombatSkillScriptsEvent(
                    combatSystemConfigurationFile.SkillScripts.File
                )
            );
            // Load GUI's
            foreach (var guiFile in combatSystemConfigurationFile.GuiList)
            {
                await _mediator.Publish(
                    new LoadCombatSystemGuiEvent(
                        guiFile.File
                    )
                );
            }
            // TODO: Load Particles
            foreach (var particleFile in combatSystemConfigurationFile.ParticleList)
            {
                await _mediator.Publish(
                    new LoadCombatParticleSystemEvent(
                        particleFile.File
                    )
                );
            }
        }
    }
}