using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.I18n.Fetch;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Game.Server.Zone.Particle.Fetch;
using EventHorizon.Performance;
using EventHorizon.Zone.System.Client.Scripts.Fetch;
using EventHorizon.Zone.System.ClientAssets.Fetch;
using EventHorizon.Zone.System.ClientEntities.Fetch;
using EventHorizon.Zone.System.EntityModule.Fetch;
using EventHorizon.Zone.System.Gui.Events.Layout;
using EventHorizon.Zone.System.ServerModule.Fetch;
using MediatR;
using EventHorizon.Zone.Core.Model.Map;

namespace EventHorizon.Game.Server.Zone.Info.Query
{
    public struct QueryForPlayerZoneInfoHandler : IRequestHandler<QueryForPlayerZoneInfo, IDictionary<string, object>>
    {
        readonly IMediator _mediator;
        readonly IMapGraph _map;
        readonly IMapMesh _mapMesh;
        readonly IEntityRepository _entityRepository;
        readonly IPerformanceTracker _performanceTracker;
        public QueryForPlayerZoneInfoHandler(
            IMediator mediator,
            IMapGraph map,
            IMapMesh mapMesh,
            IEntityRepository entityRepository,
            IPerformanceTracker performanceTracker
        )
        {
            _mediator = mediator;
            _map = map;
            _mapMesh = mapMesh;
            _entityRepository = entityRepository;
            _performanceTracker = performanceTracker;
        }

        public async Task<IDictionary<string, object>> Handle(
            QueryForPlayerZoneInfo request,
            CancellationToken cancellationToken
        )
        {
            using (_performanceTracker.Track("Zone Info For Player Created"))
            {
                var zoneInfo = new Dictionary<string, object>();
                zoneInfo.Add(
                    "Player",
                    request.Player
                );
                zoneInfo.Add(
                    "I18nMap",
                    await _mediator.Send(
                        new FetchI18nMapForLocaleQuery(
                            request.Player.Locale
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
                        new GetGuiLayoutListForPlayerCommand(
                            request.Player
                        )
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
                        new FetchServerModuleScriptListEvent()
                    )
                );
                zoneInfo.Add(
                    "ClientAssetList",
                    await _mediator.Send(
                        new FetchClientAssetListQuery()
                    )
                );
                zoneInfo.Add(
                    "ClientEntityInstanceList",
                    await _mediator.Send(
                        new FetchClientEntityInstanceListQuery()
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
                return zoneInfo;
            }
        }
    }
}