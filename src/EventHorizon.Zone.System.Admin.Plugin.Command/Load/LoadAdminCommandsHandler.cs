namespace EventHorizon.Zone.System.Admin.Plugin.Command.Load
{
    using global::System.IO;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using EventHorizon.Zone.Core.Events.DirectoryService;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.Core.Model.Json;
    using EventHorizon.Zone.System.Admin.Plugin.Command.Model;
    using EventHorizon.Zone.System.Admin.Plugin.Command.State;

    using MediatR;

    public class LoadAdminCommandsHandler : IRequestHandler<LoadAdminCommands>
    {
        readonly IMediator _mediator;
        readonly ServerInfo _serverInfo;
        readonly AdminCommandRepository _adminCommandRepository;
        readonly IJsonFileLoader _jsonLoader;

        public LoadAdminCommandsHandler(
            IMediator mediator,
            ServerInfo serverInfo,
            AdminCommandRepository adminCommandRepository,
            IJsonFileLoader jsonFileLoader
        )
        {
            _mediator = mediator;
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
            // Load in Commands from Admin/Commands folder
            foreach (var fileInfo in await _mediator.Send(
                new GetListOfFilesFromDirectory(
                    Path.Combine(
                        _serverInfo.AdminPath,
                        "Commands"
                    )
                )
            ))
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
    }
}
