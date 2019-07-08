using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.I18n.Fetch;
using EventHorizon.Game.Server.Zone.External.Entity;
using EventHorizon.Game.Server.Zone.Gui.Events;
using EventHorizon.Game.Server.Zone.Info.Api;
using EventHorizon.Game.Server.Zone.Info.Model;
using EventHorizon.Game.Server.Zone.Load.Map;
using EventHorizon.Game.Server.Zone.Load.Map.Model;
using EventHorizon.Game.Server.Zone.Particle.Fetch;
using EventHorizon.Game.Server.Zone.State;
using EventHorizon.Performance;
using EventHorizon.Zone.System.Client.Scripts.Fetch;
using EventHorizon.Zone.System.ClientAssets.Fetch;
using EventHorizon.Zone.System.ClientEntities.Fetch;
using EventHorizon.Zone.System.EntityModule.Fetch;
using EventHorizon.Zone.System.ServerModule.Fetch;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Info.Query
{
    public struct QueryForPlayerZoneInfoHandler : IRequestHandler<QueryForPlayerZoneInfo, IZoneInfo>
    {
        readonly IMediator _mediator;
        readonly ZoneMap _zoneMap;
        readonly IServerState _serverState;
        readonly IEntityRepository _entityRepository;
        readonly IPerformanceTracker _performanceTracker;
        public QueryForPlayerZoneInfoHandler(
            IMediator mediator,
            IZoneMapFactory zoneMapFactory,
            IServerState serverState,
            IEntityRepository entityRepository,
            IPerformanceTracker performanceTracker
        )
        {
            _mediator = mediator;
            _zoneMap = zoneMapFactory.Map;
            _serverState = serverState;
            _entityRepository = entityRepository;
            _performanceTracker = performanceTracker;
        }

        public async Task<IZoneInfo> Handle(
            QueryForPlayerZoneInfo request,
            CancellationToken cancellationToken
        )
        {
            using (_performanceTracker.Track("Zone Info For Player Created"))
            {
                return new ZoneInfo
                {
                    Player = request.Player,
                    I18nMap = await _mediator.Send(
                        new FetchI18nMapForLocaleQuery(
                            request.Player.Locale
                        )
                    ),
                    MapMesh = _zoneMap.Mesh,
                    Map = await _serverState.Map(),
                    EntityList = await _entityRepository.All(),
                    GuiLayout = await _mediator.Send(
                        new GetGuiLayoutForPlayerEvent
                        {
                            Player = request.Player
                        }
                    ),
                    ParticleTemplateList = await _mediator.Send(
                        new FetchAllParticleTemplateListEvent()
                    ),
                    ServerModuleScriptList = await _mediator.Send(
                        new FetchServerModuleScriptListEvent()
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
                    ),
                    
                    ClientScriptList = await _mediator.Send(
                        new FetchClientScriptListQuery()
                    ),
                };
            }
        }
    }
}