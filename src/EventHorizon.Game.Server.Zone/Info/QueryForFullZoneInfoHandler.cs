using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.I18n.Fetch;
using EventHorizon.Zone.System.Client.Scripts.Fetch;
using EventHorizon.Zone.System.ClientAssets.Fetch;
using EventHorizon.Zone.System.ClientEntities.Fetch;
using EventHorizon.Zone.System.EntityModule.Fetch;
using EventHorizon.Zone.System.Gui.Events.Layout;
using EventHorizon.Zone.System.ServerModule.Fetch;
using MediatR;
using System.Collections.Generic;
using EventHorizon.Zone.Core.Model.Map;
using EventHorizon.Zone.Core.Model.Entity.State;
using EventHorizon.Performance;
using EventHorizon.Zone.System.Particle.Fetch;

namespace EventHorizon.Game.Server.Zone.Info.Query
{
    /// <summary>
    /// This will eventually be moved into Loaders per the 
    ///  module that contains state that should be sent 
    ///  during a Zone's Full Info Request.
    /// </summary>
    public class QueryForFullZoneInfoHandler : IRequestHandler<QueryForFullZoneInfo, IDictionary<string, object>>
    {
        readonly IMediator _mediator;
        readonly IMapGraph _map;
        readonly IMapMesh _mapMesh;
        readonly EntityRepository _entityRepository;
        readonly IPerformanceTracker _performanceTracker;
        
        public QueryForFullZoneInfoHandler(
            IMediator mediator,
            IMapGraph map,
            IMapMesh mapMesh,
            EntityRepository entityRepository,
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
            QueryForFullZoneInfo request,
            CancellationToken cancellationToken
        )
        {
            using (_performanceTracker.Track("Full Zone Info Created"))
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