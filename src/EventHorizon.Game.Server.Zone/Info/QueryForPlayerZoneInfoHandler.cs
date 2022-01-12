namespace EventHorizon.Game.Server.Zone.Info.Query;

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
using EventHorizon.Zone.System.ClientAssets.Query;
using EventHorizon.Zone.System.ClientEntities.Query;
using EventHorizon.Zone.System.EntityModule.Fetch;
using EventHorizon.Zone.System.Gui.Events.Layout;
using EventHorizon.Zone.System.Particle.Fetch;
using EventHorizon.Zone.System.Player.Events.Info;
using EventHorizon.Zone.System.ServerModule.Fetch;

using MediatR;

/// <summary>
/// This will eventually be moved into Loaders per the 
///  module that contains state that should be sent 
///  during a Zone's Player Info Request.
/// </summary>
public class QueryForPlayerZoneInfoHandler
    : IRequestHandler<QueryForPlayerZoneInfo, IDictionary<string, object>>
{
    private readonly IMediator _mediator;
    private readonly IMapMesh _mapMesh;
    private readonly EntityRepository _entityRepository;
    private readonly PerformanceTrackerFactory _performanceTrackerFactory;

    public QueryForPlayerZoneInfoHandler(
        IMediator mediator,
        IMapMesh mapMesh,
        EntityRepository entityRepository,
        PerformanceTrackerFactory performanceTrackerFactory
    )
    {
        _mediator = mediator;
        _mapMesh = mapMesh;
        _entityRepository = entityRepository;
        _performanceTrackerFactory = performanceTrackerFactory;
    }

    public async Task<IDictionary<string, object>> Handle(
        QueryForPlayerZoneInfo request,
        CancellationToken cancellationToken
    )
    {
        using (_performanceTrackerFactory.Build("Zone Info For Player Created"))
        {
            var zoneInfo = new Dictionary<string, object>
            {
                { "Player", request.Player },
                {
                    "I18nMap",
                    await _mediator.Send(
                        new FetchI18nMapForLocaleQuery(request.Player.Locale),
                        cancellationToken
                    )
                },
                //zoneInfo.Add(
                //    "Map",
                //    _map
                //);
                { "MapMesh", _mapMesh },
                { "EntityList", await _entityRepository.All() },
                {
                    "GuiLayoutList",
                    await _mediator.Send(
                        new GetGuiLayoutListForPlayerCommand(request.Player),
                        cancellationToken
                    )
                },
                {
                    "ParticleTemplateList",
                    await _mediator.Send(new FetchAllParticleTemplateListEvent(), cancellationToken)
                },
                {
                    "ServerModuleScriptList",
                    await _mediator.Send(new FetchServerModuleScriptList(), cancellationToken)
                },
                {
                    "ClientAssetList",
                    await _mediator.Send(new QueryForClientAssetList(), cancellationToken)
                },
                {
                    "ClientEntityList",
                    await _mediator.Send(
                        new QueryForAllClientEntityDetailsList(),
                        cancellationToken
                    )
                },
                {
                    "BaseEntityScriptModuleList",
                    await _mediator.Send(new FetchBaseModuleListQuery(), cancellationToken)
                },
                {
                    "PlayerEntityScriptModuleList",
                    await _mediator.Send(new FetchPlayerModuleListQuery(), cancellationToken)
                },
                {
                    "ClientScriptList",
                    await _mediator.Send(new FetchClientScriptListQuery(), cancellationToken)
                },
                {
                    "ClientScriptsAssemblyDetails",
                    await _mediator.Send(
                        new QueryForClientScriptsAssemblyDetails(),
                        cancellationToken
                    )
                },
                // Game Specific State
                {
                    "GameState",
                    await _mediator.Send(new QueryForCurrentGameState(), cancellationToken)
                }
            };

            return zoneInfo;
        }
    }
}
