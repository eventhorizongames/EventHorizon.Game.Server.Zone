using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Agent.Connection;
using EventHorizon.Game.Server.Zone.Agent.Mapper;
using EventHorizon.Game.Server.Zone.Agent.Model;
using EventHorizon.Game.Server.Zone.Agent.Register;
using EventHorizon.Game.Server.Zone.Core.Json;
using EventHorizon.Game.Server.Zone.Entity.Register;
using EventHorizon.Game.Server.Zone.External.Json;
using EventHorizon.Game.Server.Zone.Load.Settings.Model;
using EventHorizon.Game.Server.Zone.Player.Mapper;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using IOPath = System.IO.Path;

namespace EventHorizon.Game.Server.Zone.Agent.Startup
{
    public class LoadZoneAgentStateHandler : IRequestHandler<LoadZoneAgentStateEvent, Unit>
    {
        readonly IMediator _mediator;
        readonly IHostingEnvironment _hostingEnvironment;
        readonly ZoneSettings _zoneSettings;
        readonly IAgentConnection _agentConnection;
        readonly IJsonFileLoader _fileLoader;

        public LoadZoneAgentStateHandler(
            IMediator mediator,
            IHostingEnvironment hostingEnvironment,
            ZoneSettings zoneSettings,
            IAgentConnection agentConnection,
            IJsonFileLoader fileLoader
        )
        {
            _mediator = mediator;
            _hostingEnvironment = hostingEnvironment;
            _zoneSettings = zoneSettings;
            _agentConnection = agentConnection;
            _fileLoader = fileLoader;
        }
        public async Task<Unit> Handle(LoadZoneAgentStateEvent request, CancellationToken cancellationToken)
        {
            // Load "GLOBAL" Agents from Agent service.
            foreach (var agent in await _agentConnection.GetAgentList(_zoneSettings.Tag))
            {
                await _mediator.Send(new RegisterAgentEvent
                {
                    Agent = AgentFromDetailsToEntity.MapToNewGlobal(agent),
                });
            }
            // Load "LOCALE" agents from file service
            var agentState = await _fileLoader.GetFile<AgentSaveState>(GetAgentFileName());
            foreach (var agent in agentState.AgentList)
            {
                await _mediator.Send(new RegisterAgentEvent
                {
                    Agent = AgentFromDetailsToEntity.MapToNew(agent),
                });
            }
            return Unit.Value;
        }
        private string GetAgentFileName()
        {
            return IOPath.Combine(_hostingEnvironment.ContentRootPath, "App_Data", "Agent.state.json");
        }
    }
}