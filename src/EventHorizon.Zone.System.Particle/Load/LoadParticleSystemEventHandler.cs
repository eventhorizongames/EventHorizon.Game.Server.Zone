namespace EventHorizon.Zone.System.Particle.Load
{
    using EventHorizon.Zone.Core.Events.FileService;
    using EventHorizon.Zone.Core.Model.FileService;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.Core.Model.Json;
    using EventHorizon.Zone.System.Particle.Events.Add;
    using EventHorizon.Zone.System.Particle.Model.Template;

    using global::System.Collections.Generic;
    using global::System.IO;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    public class LoadParticleSystemEventHandler
        : INotificationHandler<LoadParticleSystemEvent>
    {
        private readonly IMediator _mediator;
        private readonly IJsonFileLoader _fileLoader;
        private readonly ServerInfo _serverInfo;

        public LoadParticleSystemEventHandler(
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
            LoadParticleSystemEvent notification,
            CancellationToken cancellationToken
        ) => _mediator.Send(
            new ProcessFilesRecursivelyFromDirectory(
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
            var rootPath = (string)arguments["RootPath"];
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
