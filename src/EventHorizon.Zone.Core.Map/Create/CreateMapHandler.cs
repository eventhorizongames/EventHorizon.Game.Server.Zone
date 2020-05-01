namespace EventHorizon.Zone.Core.Map.Create
{
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using EventHorizon.Performance;
    using EventHorizon.Zone.Core.Events.FileService;
    using EventHorizon.Zone.Core.Events.Map.Create;
    using EventHorizon.Zone.Core.Events.Map.Generate;
    using EventHorizon.Zone.Core.Map.Model;
    using EventHorizon.Zone.Core.Map.State;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.Core.Model.Json;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public class CreateMapHandler 
        : INotificationHandler<CreateMapEvent>
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly ServerInfo _serverInfo;
        private readonly IJsonFileLoader _fileLoader;
        private readonly IServerMap _serverMap;
        private readonly IPerformanceTracker _performanceTracker;

        public CreateMapHandler(
            ILogger<CreateMapHandler> logger,
            IMediator mediator,
            ServerInfo serverInfo,
            IJsonFileLoader fileLoader,
            IServerMap serverMap,
            IPerformanceTracker performanceTracker
        )
        {
            _logger = logger;
            _mediator = mediator;
            _serverInfo = serverInfo;
            _fileLoader = fileLoader;
            _serverMap = serverMap;
            _performanceTracker = performanceTracker;
        }

        public async Task Handle(
            CreateMapEvent notification,
            CancellationToken cancellationToken
        )
        {
            // Load in ZoneMapFile
            using (_performanceTracker.Track(
                "Create Map"
            ))
            {
                var zoneMap = await GetZoneMapDetails();
                var generatedMapGraph = await _mediator.Send(
                    new GenerateMapFromDetails(
                        zoneMap
                    )
                );
                _serverMap.SetMap(generatedMapGraph);
                _serverMap.SetMapDetails(zoneMap);
                _serverMap.SetMapMesh(zoneMap.Mesh);
            }
            await _mediator.Publish(new MapCreatedEvent());
        }

        private async Task<ZoneMapDetails> GetZoneMapDetails()
        {
            var stateFile = GetStateFileName();
            if (!await _mediator.Send(
                new DoesFileExist(
                    stateFile
                )
            ))
            {
                _logger.LogError(
                    "Failed to load Zone Map Details. {ZoneMapDetailsFilePath}",
                    stateFile
                );
                return default;
            }
            return await _fileLoader.GetFile<ZoneMapDetails>(
                stateFile
            );
        }

        private string GetStateFileName()
        {
            return Path.Combine(
                _serverInfo.AppDataPath,
                "Map.state.json"
            );
        }
    }
}