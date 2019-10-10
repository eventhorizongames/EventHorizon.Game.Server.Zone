using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.Core.Model.Json;
using EventHorizon.Zone.System.Admin.Plugin.Command.Model.Scripts;
using EventHorizon.Zone.System.Admin.Plugin.Command.Scripts.State;
using MediatR;

namespace EventHorizon.Zone.System.Admin.Plugin.Command.Scripts.Load
{
    public struct LoadAdminCommandsHandler : IRequestHandler<LoadAdminCommands>
    {
        readonly ServerInfo _serverInfo;
        readonly AdminCommandRepository _adminCommandRepository;
        readonly IJsonFileLoader _jsonLoader;

        public LoadAdminCommandsHandler(
            ServerInfo serverInfo,
            AdminCommandRepository adminCommandRepository,
            IJsonFileLoader jsonFileLoader
        )
        {
            _serverInfo = serverInfo;
            _adminCommandRepository = adminCommandRepository;
            _jsonLoader = jsonFileLoader;
        }

        public async Task<Unit> Handle(
            LoadAdminCommands request,
            CancellationToken cancellationToken
        )
        {
            // Clear out any existing admin Commands
            _adminCommandRepository.Clear();
            // Load in Commands from App_Data/Admin/Commands folder
            var commandDirectory = new DirectoryInfo(
                GetAdminCommandsPath()
            );
            foreach (var fileInfo in commandDirectory.GetFiles())
            {
                // Create Get From Json File AND Add to Repository
                _adminCommandRepository.Add(
                    await _jsonLoader.GetFile<AdminCommandInstance>(
                        fileInfo.FullName
                    )
                );
            }
            return Unit.Value;
        }
        private string GetAdminCommandsPath()
        {
            return Path.Combine(
                _serverInfo.AdminPath,
                "Commands"
            );
        }
    }
}