using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.I18n.Fetch;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Game.Server.Zone.Info.Api;
using EventHorizon.Game.Server.Zone.Info.Model;
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
    public struct QueryForFullZoneInfoHandler : IRequestHandler<QueryForFullZoneInfo, IZoneInfo>
    {
        readonly IMediator _mediator;
        readonly IMapGraph _map;
        readonly IMapMesh _mapMesh;
        readonly IEntityRepository _entityRepository;
        readonly IPerformanceTracker _performanceTracker;
        public QueryForFullZoneInfoHandler(
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

        public async Task<IZoneInfo> Handle(
            QueryForFullZoneInfo request,
            CancellationToken cancellationToken
        )
        {
            using (_performanceTracker.Track("Full Zone Info Created"))
            {
                return new ZoneInfo
                {
                    I18nMap = await _mediator.Send(
                        new FetchI18nMapForLocaleQuery(
                            "en_US" // TODO: In the future change this to the requests locale
                        )
                    ),
                    MapMesh = _mapMesh,
                    Map = _map,
                    EntityList = await _entityRepository.All(),
                    GuiLayoutList = await _mediator.Send(
                        new GetGuiLayoutListForPlayerCommand()
                    ),
                    ParticleTemplateList = await _mediator.Send(
                        new FetchAllParticleTemplateListEvent()
                    ),
                    ServerModuleScriptList = await _mediator.Send(
                        new FetchServerModuleScriptListEvent()
                    ),
                    ClientScriptList = await _mediator.Send(
                        new FetchClientScriptListQuery()
                    ),
                    ClientAssetList = await _mediator.Send(
                        new FetchClientAssetListQuery()
                    ),
                    ClientEntityInstanceList = await _mediator.Send(
                        new FetchClientEntityInstanceListQuery()
                    ),
                    BaseEntityScriptModuleList = await _mediator.Send(
                        new FetchBaseModuleListQuery()
                    ),
                    PlayerEntityScriptModuleList = await _mediator.Send(
                        new FetchPlayerModuleListQuery()
                    )
                };
            }
        }
    }
}