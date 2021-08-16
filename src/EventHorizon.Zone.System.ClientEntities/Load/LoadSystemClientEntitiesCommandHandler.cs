namespace EventHorizon.Zone.System.ClientEntities.Load
{
    using EventHorizon.Zone.Core.Events.DirectoryService;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.Core.Model.Json;
    using EventHorizon.Zone.System.Agent.Save.Mapper;
    using EventHorizon.Zone.System.ClientEntities.Model;
    using EventHorizon.Zone.System.ClientEntities.Query;
    using EventHorizon.Zone.System.ClientEntities.Register;
    using EventHorizon.Zone.System.ClientEntities.Unregister;

    using global::System.Collections.Generic;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    /// <summary>
    /// TODO: Load Recursively from root path
    /// This will require update to the Plugin Editor for ClientEntities
    /// </summary>
    public class LoadSystemClientEntitiesCommandHandler : IRequestHandler<LoadSystemClientEntitiesCommand>
    {
        private readonly IMediator _mediator;
        private readonly IJsonFileLoader _fileLoader;
        private readonly ServerInfo _serverInfo;

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
            // Unregister any existing
            var existingEntityList = await _mediator.Send(
                new QueryForAllClientEntityDetailsList()
            );
            foreach (var entity in existingEntityList)
            {
                await _mediator.Send(
                    new UnregisterClientEntity(
                        entity.GlobalId
                    )
                );
            }
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
                var client = await _fileLoader.GetFile<ClientEntityDetails>(
                    fileInfo.FullName
                );
                client.Data[ClientEntityConstants.METADATA_FILE_FULL_NAME] = fileInfo.FullName;
                result.Add(
                    client
                );
            }
            return result;
        }
    }
}
