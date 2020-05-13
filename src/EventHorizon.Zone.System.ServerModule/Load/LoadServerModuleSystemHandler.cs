namespace EventHorizon.Zone.System.ServerModule.Load
{
    using EventHorizon.Zone.Core.Events.DirectoryService;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.Core.Model.Json;
    using EventHorizon.Zone.System.ServerModule.Model;
    using EventHorizon.Zone.System.ServerModule.State;
    using global::System.IO;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;

    /// <summary>
    /// TODO: Make this recursive Loading
    /// </summary>
    public class LoadServerModuleSystemHandler
        : INotificationHandler<LoadServerModuleSystem>
    {
        private readonly IMediator _mediator;
        private readonly ServerInfo _serverInfo;
        private readonly IJsonFileLoader _fileLoader;
        private readonly ServerModuleRepository _serverModuleRepository;

        public LoadServerModuleSystemHandler(
            IMediator mediator,
            ServerInfo serverInfo,
            IJsonFileLoader fileLoader,
            ServerModuleRepository serverModuleRepository
        )
        {
            _mediator = mediator;
            _serverInfo = serverInfo;
            _fileLoader = fileLoader;
            _serverModuleRepository = serverModuleRepository;
        }

        public async Task Handle(
            LoadServerModuleSystem notification,
            CancellationToken cancellationToken
        )
        {
            await GetServerModuleList(
                GetServerScriptsPath()
            );
        }

        private string GetServerScriptsPath()
        {
            return Path.Combine(
                _serverInfo.ClientPath,
                "ServerModule"
            );
        }

        private async Task GetServerModuleList(
            string path
        )
        {
            foreach (var file in await _mediator.Send(
                new GetListOfFilesFromDirectory(
                    path
                )
            ))
            {
                AddServerModuleScript(
                    await GetServerModuleFile(
                        file.FullName
                    )
                );
            }
        }

        private Task<ServerModuleScripts> GetServerModuleFile(
            string fullName
        )
        {
            return _fileLoader.GetFile<ServerModuleScripts>(
                fullName
            );
        }

        private void AddServerModuleScript(
            ServerModuleScripts serverModule
        )
        {
            _serverModuleRepository.Add(
                serverModule
            );
        }
    }
}