namespace EventHorizon.Zone.System.Agent.Save
{
    using global::System.Collections.Generic;
    using global::System.IO;
    using global::System.Threading;
    using global::System.Threading.Tasks;

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

    public class SaveAgentStateHandler : INotificationHandler<SaveAgentStateEvent>
    {
        readonly ServerInfo _serverInfo;
        readonly IJsonFileSaver _fileSaver;
        readonly IAgentRepository _agentRepository;

        readonly IAgentConnection _agentConnection;
        readonly PerformanceTrackerFactory _performanceTrackerFactory;

        public SaveAgentStateHandler(
            ServerInfo serverInfo,
            IJsonFileSaver fileSaver,
            IAgentRepository agentRepository,
            IAgentConnection agentConnection,
            PerformanceTrackerFactory performanceTrackerFactory
        )
        {
            _serverInfo = serverInfo;
            _fileSaver = fileSaver;
            _agentRepository = agentRepository;
            _agentConnection = agentConnection;
            _performanceTrackerFactory = performanceTrackerFactory;
        }
        public async Task Handle(
            SaveAgentStateEvent notification,
            CancellationToken cancellationToken
        )
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
        private string GetAgentDataDirectory()
        {
            return Path.Combine(
                _serverInfo.AppDataPath,
                "Agent"
            );
        }
        private string GetAgentFileName()
        {
            return "Agent.state.json";
        }
    }
}
