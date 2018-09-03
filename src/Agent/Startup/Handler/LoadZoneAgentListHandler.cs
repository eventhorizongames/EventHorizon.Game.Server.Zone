using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Agent.Mapper;
using EventHorizon.Game.Server.Zone.Agent.Model;
using EventHorizon.Game.Server.Zone.Agent.Register;
using EventHorizon.Game.Server.Zone.Core.Json;
using EventHorizon.Game.Server.Zone.Entity.Register;
using EventHorizon.Game.Server.Zone.Player.Mapper;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using IOPath = System.IO.Path;

namespace EventHorizon.Game.Server.Zone.Agent.Startup.Handler
{
    public class LoadZoneAgentStateHandler : IRequestHandler<LoadZoneAgentStateEvent, Unit>
    {
        readonly IMediator _mediator;
        readonly IHostingEnvironment _hostingEnvironment;
        readonly IJsonFileLoader _fileLoader;

        public LoadZoneAgentStateHandler(IMediator mediator,
            IHostingEnvironment hostingEnvironment,
            IJsonFileLoader jsonFileLoader)
        {
            _mediator = mediator;
            _hostingEnvironment = hostingEnvironment;
            _fileLoader = jsonFileLoader;
        }
        public async Task<Unit> Handle(LoadZoneAgentStateEvent request, CancellationToken cancellationToken)
        {
            // TODO: Move loading to a Persistence service
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