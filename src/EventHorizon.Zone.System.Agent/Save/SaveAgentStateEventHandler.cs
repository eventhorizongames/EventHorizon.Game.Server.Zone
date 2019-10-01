using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Performance;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.Core.Model.Json;
using EventHorizon.Zone.System.Agent.Connection;
using EventHorizon.Zone.System.Agent.Connection.Model;
using EventHorizon.Zone.System.Agent.Model.State;
using EventHorizon.Zone.System.Agent.Save.Events;
using EventHorizon.Zone.System.Agent.Save.Mapper;
using EventHorizon.Zone.System.Agent.Save.Model;
using MediatR;

namespace EventHorizon.Zone.System.Agent.Save
{
    public class SaveAgentStateHandler : INotificationHandler<SaveAgentStateEvent>
    {
        readonly ServerInfo _serverInfo;
        readonly IJsonFileSaver _fileSaver;
        readonly IAgentRepository _agentRepository;

        readonly IAgentConnection _agentConnection;
        readonly IPerformanceTracker _performanceTracker;

        public SaveAgentStateHandler(
            ServerInfo serverInfo,
            IJsonFileSaver fileSaver,
            IAgentRepository agentRepository,
            IAgentConnection agentConnection,
            IPerformanceTracker performanceTracker
        )
        {
            _serverInfo = serverInfo;
            _fileSaver = fileSaver;
            _agentRepository = agentRepository;
            _agentConnection = agentConnection;
            _performanceTracker = performanceTracker;
        }
        public async Task Handle(
            SaveAgentStateEvent notification,
            CancellationToken cancellationToken
        )
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
                            AgentFromEntityToDetails.Map(
                                agent
                            )
                        ).ConfigureAwait(
                            false
                        ).GetAwaiter();
                    }
                    else
                    {
                        saveAgentList.Add(
                            AgentFromEntityToDetails.Map(
                                agent
                            )
                        );
                    }
                }

                // Save "LOCAL" agents
                await _fileSaver.SaveToFile(
                    GetAgentDataDirectory(),
                    GetAgentFileName(),
                    new AgentSaveState
                    {
                        AgentList = saveAgentList
                    }
                );
            }
        }
        private string GetAgentDataDirectory()
        {
            return _serverInfo.AppDataPath;
        }
        private string GetAgentFileName()
        {
            return "Agent.state.json";
        }
    }
}