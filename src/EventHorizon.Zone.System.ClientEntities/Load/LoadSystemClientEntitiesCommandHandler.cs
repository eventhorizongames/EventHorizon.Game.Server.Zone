namespace EventHorizon.Zone.System.ClientEntities.Load
{
    using global::System.Collections.Generic;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using EventHorizon.Zone.Core.Events.DirectoryService;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.Core.Model.Json;
    using EventHorizon.Zone.System.ClientEntities.Register;
    using MediatR;
    using EventHorizon.Zone.System.ClientEntities.Model;
    using EventHorizon.Zone.System.Agent.Save.Mapper;

    /// <summary>
    /// TODO: Load Recursively from root path
    /// This will require update to the Plugin Editor for ClientEntities
    /// </summary>
    public class LoadSystemClientEntitiesCommandHandler : IRequestHandler<LoadSystemClientEntitiesCommand>
    {
        readonly IMediator _mediator;
        readonly IJsonFileLoader _fileLoader;
        readonly ServerInfo _serverInfo;

        public LoadSystemClientEntitiesCommandHandler(
            IMediator mediator,
            IJsonFileLoader fileLoader,
            ServerInfo serverInfo
        )
        {
            _mediator = mediator;
            _fileLoader = fileLoader;
            _serverInfo = serverInfo;
        }

        public async Task<Unit> Handle(
            LoadSystemClientEntitiesCommand request,
            CancellationToken cancellationToken
        )
        {
            // Register ClientEntities from Path
            foreach (var clientEntityDetails in await GetClientEntitiesFromPath(
                _serverInfo.ClientEntityPath
            ))
            {
                // Register clientEntity from File
                await _mediator.Send(
                    new RegisterClientEntityCommand(
                        ClientEntityFromDetailsToEntity.Map(
                            clientEntityDetails
                        )
                    )
                );
            }
            return Unit.Value;
        }
        private async Task<IList<ClientEntityDetails>> GetClientEntitiesFromPath(
            string path
        )
        {
            var result = new List<ClientEntityDetails>();
            foreach (var fileInfo in await _mediator.Send(
                new GetListOfFilesFromDirectory(
                    path
                )
            ))
            {
                result.Add(
                    await _fileLoader.GetFile<ClientEntityDetails>(
                        fileInfo.FullName
                    )
                );
            }
            return result;
        }
    }
}