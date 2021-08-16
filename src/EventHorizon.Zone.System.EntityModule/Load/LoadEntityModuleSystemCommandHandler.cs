using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.Core.Model.Json;
using EventHorizon.Zone.System.EntityModule.Api;
using EventHorizon.Zone.System.EntityModule.Model;

using MediatR;

namespace EventHorizon.Zone.System.EntityModule.Load
{
    public class LoadEntityModuleSystemCommandHandler : INotificationHandler<LoadEntityModuleSystemCommand>
    {
        readonly IMediator _mediator;
        readonly ServerInfo _serverInfo;
        readonly IJsonFileLoader _fileLoader;
        readonly EntityModuleRepository _entityModuleRepository;

        public LoadEntityModuleSystemCommandHandler(
            IMediator mediator,
            ServerInfo serverInfo,
            IJsonFileLoader fileLoader,
            EntityModuleRepository entityModuleRepository
        )
        {
            _mediator = mediator;
            _serverInfo = serverInfo;
            _fileLoader = fileLoader;
            _entityModuleRepository = entityModuleRepository;
        }

        public async Task Handle(LoadEntityModuleSystemCommand notification, CancellationToken cancellationToken)
        {
            // Load BaseModule list into repository
            foreach (var baseModule in await GetModuleList(GetModulePathForType("Base")))
            {
                _entityModuleRepository.AddBaseModule(
                    baseModule
                );
            }
            // Load PlayerModule list into repository
            foreach (var playerModule in await GetModuleList(GetModulePathForType("Player")))
            {
                _entityModuleRepository.AddPlayerModule(
                    playerModule
                );
            }
        }
        private string GetModulePathForType(string type)
        {
            return Path.Combine(
                _serverInfo.ClientPath,
                "Modules",
                type
            );
        }
        private async Task<IList<EntityScriptModule>> GetModuleList(
            string modulePath
        )
        {
            var result = new List<EntityScriptModule>();
            var directoryInfo = new DirectoryInfo(modulePath);
            if (!directoryInfo.Exists)
            {
                return result;
            }
            foreach (var fileInfo in directoryInfo.GetFiles())
            {
                result.Add(
                    await _fileLoader.GetFile<EntityScriptModule>(
                        fileInfo.FullName
                    )
                );
            }
            return result;
        }
    }
}
