using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Agent.Connection;
using EventHorizon.Zone.System.Agent.Mapper;
using EventHorizon.Zone.System.Agent.Model;
using EventHorizon.Zone.Core.Model.Json;
using EventHorizon.Game.Server.Zone.Load.Settings.Model;
using EventHorizon.Performance;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using IOPath = System.IO.Path;
using EventHorizon.Zone.System.Agent.Model.State;

namespace EventHorizon.Zone.System.Agent.Save.Handler
{
    public class SaveAgentStateHandler : INotificationHandler<SaveAgentStateEvent>
    {
        readonly ZoneSettings _zoneSettings;
        readonly IJsonFileSaver _fileSaver;
        readonly IAgentRepository _agentRepository;
        readonly IHostingEnvironment _hostingEnvironment;

        readonly IAgentConnection _agentConnection;
        readonly IPerformanceTracker _performanceTracker;

        public SaveAgentStateHandler(
            ZoneSettings zoneSettings,
            IJsonFileSaver fileSaver,
            IAgentRepository agentRepository,
            IHostingEnvironment hostingEnvironment,
            IAgentConnection agentConnection,
            IPerformanceTracker performanceTracker
        )
        {
            _zoneSettings = zoneSettings;
            _fileSaver = fileSaver;
            _agentRepository = agentRepository;
            _hostingEnvironment = hostingEnvironment;
            _agentConnection = agentConnection;
            _performanceTracker = performanceTracker;
        }
        public async Task Handle(SaveAgentStateEvent notification, CancellationToken cancellationToken)
        {
            using (var tracker = _performanceTracker.Track("Saving Agent State"))
            {

                var saveAgentList = new List<AgentDetails>();
                foreach (var agent in await _agentRepository.All())
                {
                    // Update "GLOBAL" agents, Add "LOCAL" to list to be saved.
                    if (agent.IsGlobal)
                    {
                        _agentConnection.UpdateAgent(
                            AgentFromEntityToDetails.Map(agent)
                        ).ConfigureAwait(false).GetAwaiter();
                    }
                    else
                    {
                        saveAgentList.Add(
                            AgentFromEntityToDetails.Map(agent)
                        );
                    }
                }

                // Save "LOCAL" agents
                await _fileSaver.SaveToFile(GetAgentDataDirectory(), GetAgentFileName(), new AgentSaveState
                {
                    AgentList = saveAgentList
                });
            }
        }
        private string GetAgentDataDirectory()
        {
            return IOPath.Combine(_hostingEnvironment.ContentRootPath, "App_Data");
        }
        private string GetAgentFileName()
        {
            return "Agent.state.json";
        }
    }
}