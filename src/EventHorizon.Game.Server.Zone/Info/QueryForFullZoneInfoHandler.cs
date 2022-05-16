namespace EventHorizon.Game.Server.Zone.Info.Query;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using EventHorizon.Game.I18n.Fetch;
using EventHorizon.Performance;
using EventHorizon.Zone.Core.Model.Entity.State;
using EventHorizon.Zone.Core.Model.Map;
using EventHorizon.Zone.System.Client.Scripts.Events.Fetch;
using EventHorizon.Zone.System.Client.Scripts.Events.Query;
using EventHorizon.Zone.System.ClientAssets.Query;
using EventHorizon.Zone.System.ClientEntities.Query;
using EventHorizon.Zone.System.EntityModule.Fetch;
using EventHorizon.Zone.System.Gui.Events.Layout;
using EventHorizon.Zone.System.Particle.Query;
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
    private readonly IMediator _mediator;
    private readonly IMapGraph _map;
    private readonly IMapMesh _mapMesh;
    private readonly EntityRepository _entityRepository;
    private readonly PerformanceTrackerFactory _performanceTrackerFactory;

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
            var zoneInfo = new Dictionary<string, object>
            {
                {
                    "I18nMap",
                    await _mediator.Send(
                        new FetchI18nMapForLocaleQuery(
                            "en_US" // TODO: In the future change this to the requests locale
                        ),
                        cancellationToken
                    )
                },
                { "Map", _map },
                { "MapMesh", _mapMesh },
                { "EntityList", await _entityRepository.All() },
                {
                    "GuiLayoutList",
                    await _mediator.Send(new GetGuiLayoutListForPlayerCommand(), cancellationToken)
                },
                {
                    "ParticleTemplateList",
                    await _mediator.Send(new QueryForAllParticleTemplates(), cancellationToken)
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
                        new QueryForAllRawClientEntityDetailsList(),
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
            };

            return zoneInfo;
        }
    }
}
