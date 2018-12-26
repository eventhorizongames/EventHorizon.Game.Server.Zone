using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.External.Info;
using EventHorizon.Game.Server.Zone.External.Json;
using EventHorizon.Zone.System.ServerModule.Model;
using EventHorizon.Zone.System.ServerModule.State;
using MediatR;

namespace EventHorizon.Zone.System.ServerModule.Load
{
    public class LoadServerModuleSystemHandler : INotificationHandler<LoadServerModuleSystemEvent>
    {
        readonly IMediator _mediator;
        readonly ServerInfo _serverInfo;
        readonly IJsonFileLoader _fileLoader;
        readonly ServerModuleRepository _serverModuleRepository;

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

        public async Task Handle(LoadServerModuleSystemEvent notification, CancellationToken cancellationToken)
        {
            foreach (var serverModule in await GetServerModuleList())
            {
                _serverModuleRepository.Add(
                    serverModule.CreateScripts(
                        GetServerScriptsPath()
                    )
                );
            }
        }
        private string GetServerScriptsPath()
        {
            return Path.Combine(
                _serverInfo.ScriptsPath,
                "Server"
            );
        }
        private async Task<IEnumerable<ServerModuleDefintion>> GetServerModuleList()
        {
            return (await GetServerModuleFile()).ServerModuleList;
        }
        private Task<ServerModuleFile> GetServerModuleFile()
        {
            return _fileLoader.GetFile<ServerModuleFile>(
                Path.Combine(_serverInfo.SystemsPath, "System.ServerModule.json")
            );
        }
    }
}