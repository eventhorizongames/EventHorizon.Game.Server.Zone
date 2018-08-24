using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Agent.Mapper;
using EventHorizon.Game.Server.Zone.Agent.Model;
using EventHorizon.Game.Server.Zone.Agent.Register;
using EventHorizon.Game.Server.Zone.Entity.Register;
using EventHorizon.Game.Server.Zone.Player.Mapper;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;

namespace EventHorizon.Game.Server.Zone.Agent.Startup.Handler
{
    public class LoadZoneAgentStateHandler : IRequestHandler<LoadZoneAgentStateEvent, int>
    {
        readonly IMediator _mediator;
        readonly IHostingEnvironment _hostingEnvironment;
        public LoadZoneAgentStateHandler(IMediator mediator, IHostingEnvironment hostingEnvironment)
        {
            _mediator = mediator;
            _hostingEnvironment = hostingEnvironment;
        }
        public async Task<int> Handle(LoadZoneAgentStateEvent request, CancellationToken cancellationToken)
        {
            // TODO: Move loading to a Persistence service
            if (AgentFileExists())
            {
                using (var settingsFile = File.OpenText(GetAgentFileName()))
                {
                    var agentState = JsonConvert.DeserializeObject<AgentSaveState>(await settingsFile.ReadToEndAsync());

                    foreach (var agent in agentState.AgentList)
                    {
                        await _mediator.Send(new RegisterAgentEvent
                        {
                            Agent = AgentFromDetailsToEntity.MapToNew(agent),
                        });
                    }
                }
            }
            return 1;
        }
        private bool AgentFileExists()
        {
            return File.Exists(GetAgentFileName());
        }
        private string GetAgentFileName()
        {
            return $"{_hostingEnvironment.ContentRootPath}/App_Data/Agent.state.json";
        }
    }
}