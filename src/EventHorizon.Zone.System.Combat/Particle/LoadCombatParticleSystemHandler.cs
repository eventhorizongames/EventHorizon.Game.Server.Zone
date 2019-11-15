using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.FileService;
using EventHorizon.Zone.Core.Model.FileService;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.Core.Model.Json;
using EventHorizon.Zone.System.Combat.Particle.Event;
using EventHorizon.Zone.System.Particle.Events.Add;
using EventHorizon.Zone.System.Particle.Model.Template;
using MediatR;

namespace EventHorizon.Zone.System.Combat.Particle.Handler
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
        ) => _mediator.Send(
            new LoadFileRecursivelyFromDirectory(
                Path.Combine(
                    _serverInfo.ClientPath,
                    "Particle"
                ),
                OnProcessFile,
                new Dictionary<string, object>
                {
                    {
                        "RootPath",
                        $"{_serverInfo.ClientPath}{Path.DirectorySeparatorChar}"
                    }
                }
            )
        );

        private async Task OnProcessFile(
            StandardFileInfo fileInfo,
            IDictionary<string, object> arguments
        )
        {
            var rootPath = arguments["RootPath"] as string;
            var particleTemplate = await _fileLoader.GetFile<ParticleTemplate>(
                fileInfo.FullName
            );
            particleTemplate.Id = GenerateName(
                rootPath.MakePathRelative(
                    fileInfo.DirectoryName
                ),
                fileInfo.Name,
                fileInfo.Extension
            );
            await _mediator.Publish(
                new AddParticleTemplateEvent(
                    particleTemplate.Id,
                    particleTemplate
                )
            );
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