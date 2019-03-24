using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Entity.State;
using EventHorizon.Game.Server.Zone.Load;
using EventHorizon.Game.Server.Zone.Load.Map;
using EventHorizon.Game.Server.Zone.Load.Map.Model;
using EventHorizon.Game.Server.Zone.State;
using EventHorizon.Game.Server.Zone.Player.Model;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using EventHorizon.Game.Server.Zone.External.Entity;
using System.Collections.Generic;
using EventHorizon.Game.Server.Zone.Map.State;
using EventHorizon.Game.Server.Zone.Model.Entity;
using EventHorizon.Game.Server.Zone.Model.Gui;
using EventHorizon.Game.Server.Zone.Gui;
using EventHorizon.Game.Server.Zone.Gui.Get;
using EventHorizon.Game.Server.Zone.Gui.Events;
using EventHorizon.Game.Server.Zone.Model.Player;
using EventHorizon.Game.Server.Zone.Player.Bus;
using EventHorizon.Game.Server.Zone.Model.Particle;
using EventHorizon.Game.Server.Zone.Particle.Fetch;
using EventHorizon.Plugin.Zone.System.Combat.Skill.Model;
using EventHorizon.Plugin.Zone.System.Combat.Skill.Fetch;
using EventHorizon.Zone.System.ServerModule.Fetch;
using EventHorizon.Zone.System.ServerModule.Model;
using EventHorizon.Zone.System.Client.Scripts.Fetch;
using EventHorizon.Zone.System.Client.Scripts.Model;
using EventHorizon.Game.I18n.Fetch;

namespace EventHorizon.Game.Server.Zone.Player.Zone.Handler
{
    public class SendZoneInfoToPlayerHandler : IRequestHandler<SendZoneInfoToPlayerEvent, PlayerEntity>
    {
        readonly IMediator _mediator;
        readonly ZoneMap _zoneMap;
        readonly IServerState _serverState;
        readonly IEntityRepository _entityRepository;
        readonly IHubContext<PlayerHub> _hubContext;
        public SendZoneInfoToPlayerHandler(
            IMediator mediator,
            IZoneMapFactory zoneMapFactory,
            IServerState serverState,
            IEntityRepository entityRepository,
            IHubContext<PlayerHub> hubContext)
        {
            _mediator = mediator;
            _zoneMap = zoneMapFactory.Map;
            _serverState = serverState;
            _entityRepository = entityRepository;
            _hubContext = hubContext;
        }
        public async Task<PlayerEntity> Handle(SendZoneInfoToPlayerEvent request, CancellationToken cancellationToken)
        {
            var zoneInfo = new ZoneList
            {
                Player = request.Player,
                I18nMap = await _mediator.Send(new FetchI18nMapForLocaleQuery(request.Player.Locale)),
                MapMesh = _zoneMap.Mesh,
                Map = await _serverState.Map(),
                EntityList = await _entityRepository.All(),
                GuiLayout = await _mediator.Send(new GetGuiLayoutForPlayerEvent
                {
                    Player = request.Player
                }),
                ParticleTemplateList = await _mediator.Send(new FetchAllParticleTemplateListEvent()),
                SkillActionScriptList = await _mediator.Send(new FetchAllSkillActionScriptListEvent()),
                ServerModuleScriptList = await _mediator.Send(new FetchServerModuleScriptListEvent()),
                ClientScriptList = await _mediator.Send(new FetchClientScriptListQuery())
            };
            await _hubContext.Clients.Client(request.Player.ConnectionId).SendAsync("ZoneInfo", zoneInfo);

            return request.Player;
        }
    }
    public struct ZoneList
    {
        public PlayerEntity Player { get; set; }
        public IDictionary<string, string> I18nMap { get; set; }
        public ZoneMapMesh MapMesh { get; set; }
        public MapGraph Map { get; set; }
        public List<IObjectEntity> EntityList { get; set; }
        public GuiLayout GuiLayout { get; set; }
        public IEnumerable<ParticleTemplate> ParticleTemplateList { get; set; }
        public IEnumerable<SkillActionScript> SkillActionScriptList { get; set; }
        public IEnumerable<ServerModuleScripts> ServerModuleScriptList { get; set; }
        public IEnumerable<ClientScript> ClientScriptList { get; set; }
    }
}