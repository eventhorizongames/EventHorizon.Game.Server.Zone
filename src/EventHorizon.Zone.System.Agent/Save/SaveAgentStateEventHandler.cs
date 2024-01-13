namespace EventHorizon.Zone.System.Agent.Save;

using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.Core.Model.Json;
using EventHorizon.Zone.System.Agent.Connection;
using EventHorizon.Zone.System.Agent.Connection.Model;
using EventHorizon.Zone.System.Agent.Model.State;
using EventHorizon.Zone.System.Agent.Save.Events;
using EventHorizon.Zone.System.Agent.Save.Mapper;
using EventHorizon.Zone.System.Agent.Save.Model;

using global::System.Collections.Generic;
using global::System.IO;
using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

public class SaveAgentStateHandler
    : INotificationHandler<SaveAgentStateEvent>
{
    private readonly ServerInfo _serverInfo;
    private readonly IJsonFileSaver _fileSaver;
    private readonly IAgentRepository _agentRepository;
    private readonly IAgentConnection _agentConnection;

    public SaveAgentStateHandler(
        ServerInfo serverInfo,
        IJsonFileSaver fileSaver,
        IAgentRepository agentRepository,
        IAgentConnection agentConnection
    )
    {
        _serverInfo = serverInfo;
        _fileSaver = fileSaver;
        _agentRepository = agentRepository;
        _agentConnection = agentConnection;
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
