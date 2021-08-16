namespace EventHorizon.Game.Server.Zone.Info.Query
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using EventHorizon.Game.I18n.Fetch;
    using EventHorizon.Game.Query;
    using EventHorizon.Performance;
    using EventHorizon.Zone.Core.Model.Entity.State;
    using EventHorizon.Zone.Core.Model.Map;
    using EventHorizon.Zone.System.Client.Scripts.Events.Fetch;
    using EventHorizon.Zone.System.Client.Scripts.Events.Query;
    using EventHorizon.Zone.System.Client.Scripts.Fetch;
    using EventHorizon.Zone.System.ClientAssets.Fetch;
    using EventHorizon.Zone.System.ClientEntities.Query;
    using EventHorizon.Zone.System.EntityModule.Fetch;
    using EventHorizon.Zone.System.Gui.Events.Layout;
    using EventHorizon.Zone.System.Particle.Fetch;
    using EventHorizon.Zone.System.ServerModule.Fetch;

    using MediatR;

    /// <summary>
    /// This will eventually be moved into Loaders per the 
    ///  module that contains state that should be sent 
    ///  during a Zone's Full Info Request.
    /// </summary>
    public class QueryForFullZoneInfoHandler
        : IRequestHandler<QueryForFullZoneInfo, IDictionary<string, object>>
    {
        readonly IMediator _mediator;
        readonly IMapGraph _map;
        readonly IMapMesh _mapMesh;
        readonly EntityRepository _entityRepository;
        readonly PerformanceTrackerFactory _performanceTrackerFactory;

        public QueryForFullZoneInfoHandler(
            IMediator mediator,
            IMapGraph map,
            IMapMesh mapMesh,
            EntityRepository entityRepository,
            PerformanceTrackerFactory performanceTrackerFactory
        )
        {
            _mediator = mediator;
            _map = map;
            _mapMesh = mapMesh;
            _entityRepository = entityRepository;
            _performanceTrackerFactory = performanceTrackerFactory;
        }

        public async Task<IDictionary<string, object>> Handle(
            QueryForFullZoneInfo request,
            CancellationToken cancellationToken
        )
        {
            using (_performanceTrackerFactory.Build("Full Zone Info Created"))
            {
                var zoneInfo = new Dictionary<string, object>();
                zoneInfo.Add(
                    "I18nMap",
                    await _mediator.Send(
                        new FetchI18nMapForLocaleQuery(
                            "en_US" // TODO: In the future change this to the requests locale
                        )
                    )
                );
                zoneInfo.Add(
                    "Map",
                    _map
                );
                zoneInfo.Add(
                    "MapMesh",
                    _mapMesh
                );
                zoneInfo.Add(
                    "EntityList",
                    await _entityRepository.All()
                );
                zoneInfo.Add(
                    "GuiLayoutList",
                    await _mediator.Send(
                        new GetGuiLayoutListForPlayerCommand()
                    )
                );
                zoneInfo.Add(
                    "ParticleTemplateList",
                    await _mediator.Send(
                        new FetchAllParticleTemplateListEvent()
                    )
                );
                zoneInfo.Add(
                    "ServerModuleScriptList",
                    await _mediator.Send(
                        new FetchServerModuleScriptList()
                    )
                );
                zoneInfo.Add(
                    "ClientAssetList",
                    await _mediator.Send(
                        new FetchClientAssetListQuery()
                    )
                );
                zoneInfo.Add(
                    "ClientEntityList",
                    await _mediator.Send(
                        new QueryForAllRawClientEntityDetailsList()
                    )
                );
                zoneInfo.Add(
                    "BaseEntityScriptModuleList",
                    await _mediator.Send(
                        new FetchBaseModuleListQuery()
                    )
                );
                zoneInfo.Add(
                    "PlayerEntityScriptModuleList",
                    await _mediator.Send(
                        new FetchPlayerModuleListQuery()
                    )
                );
                zoneInfo.Add(
                    "ClientScriptList",
                    await _mediator.Send(
                        new FetchClientScriptListQuery()
                    )
                );
                zoneInfo.Add(
                    "ClientScriptsAssemblyDetails",
                    await _mediator.Send(
                        new QueryForClientScriptsAssemblyDetails()
                    )
                );

                // Game Specific State
                zoneInfo.Add(
                    "GameState",
                    await _mediator.Send(
                        new QueryForCurrentGameState()
                    )
                );

                return zoneInfo;
            }
        }
    }
}
