using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Events.Particle.Add;
using EventHorizon.Zone.Core.Model.Extensions;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.Core.Model.Json;
using EventHorizon.Zone.Core.Model.Particle;
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
        public LoadCombatParticleSystemHandler(
            IMediator mediator,
            IJsonFileLoader fileLoader,
            ServerInfo serverInfo
        )
        {
            _mediator = mediator;
            _fileLoader = fileLoader;
            _serverInfo = serverInfo;
        }
        public Task Handle(
            LoadCombatParticleSystemEvent notification,
            CancellationToken cancellationToken
        )
        {
            return this.LoadFromDirectoryInfo(
                _serverInfo.ClientPath,
                new DirectoryInfo(
                    Path.Combine(
                        _serverInfo.ClientPath,
                        "Particle"
                    )
                )
            );
        }
        private async Task LoadFromDirectoryInfo(
            string scriptsPath,
            DirectoryInfo directoryInfo
        )
        {
            // Load Scripts from Sub-Directories
            foreach (var subDirectoryInfo in directoryInfo.GetDirectories())
            {
                // Load Files From Directories
                await this.LoadFromDirectoryInfo(
                    scriptsPath,
                    subDirectoryInfo
                );
            }
            // Load script files into Repository
            await this.LoadFileIntoRepository(
                scriptsPath,
                directoryInfo
            );
        }
        private async Task LoadFileIntoRepository(
            string scriptsPath,
            DirectoryInfo directoryInfo
        )
        {
            foreach (var effectFile in directoryInfo.GetFiles())
            {
                var particleTemplate = await _fileLoader.GetFile<ParticleTemplate>(
                    effectFile.FullName
                );
                particleTemplate.Id = GenerateName(
                    $"{scriptsPath}{Path.DirectorySeparatorChar}".MakePathRelative(
                        effectFile.DirectoryName
                    ), effectFile.Name,
                    effectFile.Extension
                );
                await _mediator.Publish(
                    new AddParticleTemplateEvent
                    {
                        Id = particleTemplate.Id,
                        Template = particleTemplate
                    }
                );
            }
        }
        private static string GenerateName(
            string path,
            string fileName,
            string extensionToRemove
        )
        {
            return string.Join(
                "_",
                string.Join(
                    "_",
                    path.Split(
                        Path.DirectorySeparatorChar
                    )
                ),
                fileName
            ).Replace(
                extensionToRemove,
                string.Empty
            );
        }
    }
}