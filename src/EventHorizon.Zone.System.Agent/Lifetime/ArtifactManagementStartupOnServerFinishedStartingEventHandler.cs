namespace EventHorizon.Zone.System.Agent.Lifetime;

using EventHorizon.Zone.Core.Events.Lifetime;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.Core.Model.Json;
using EventHorizon.Zone.Core.Model.Settings;
using EventHorizon.Zone.System.Agent;
using EventHorizon.Zone.System.Agent.Connection;
using EventHorizon.Zone.System.Agent.Connection.Model;
using EventHorizon.Zone.System.Agent.Events.Get;
using EventHorizon.Zone.System.Agent.Events.Register;
using EventHorizon.Zone.System.Agent.Save.Mapper;
using EventHorizon.Zone.System.Agent.Save.Model;

using global::System;
using global::System.Collections.Generic;
using global::System.IO;
using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

public class AgentSystemStartupOnServerFinishedStartingEventHandler
    : INotificationHandler<ServerFinishedStartingEvent>
{
    private readonly ISender _sender;
    private readonly ServerInfo _serverInfo;
    private readonly ZoneSettings _zoneSettings;
    private readonly IAgentConnection _agentConnection;
    private readonly IJsonFileLoader _fileLoader;

    public AgentSystemStartupOnServerFinishedStartingEventHandler(
        ISender sender,
        ServerInfo serverInfo,
        ZoneSettings zoneSettings,
        IAgentConnection agentConnection,
        IJsonFileLoader fileLoader
    )
    {
        _sender = sender;
        _serverInfo = serverInfo;
        _zoneSettings = zoneSettings;
        _agentConnection = agentConnection;
        _fileLoader = fileLoader;
    }

    public async Task Handle(
        ServerFinishedStartingEvent notification,
        CancellationToken cancellationToken
    )
    {
        // UnRegister Existing Agents
        var agentList = await _sender.Send(
            new GetAgentListEvent(),
            cancellationToken
        );
        foreach (var agent in agentList)
        {
            await _sender.Send(
                new UnRegisterAgent(
                    agent.AgentId
                ),
                cancellationToken
            );
        }

        // Load "GLOBAL" Agents from Agent service.
        foreach (var agent in await _agentConnection.GetAgentList(
            _zoneSettings.Tag
        ))
        {
            await _sender.Send(
                new RegisterAgentEvent(
                    AgentFromDetailsToEntity.MapToNewGlobal(
                        agent
                    )
                ),
                cancellationToken
            );
        }
        // Load "LOCAL" agents from file service
        var agentState = await _fileLoader.GetFile<AgentSaveState>(
            GetAgentFileName()
        );

        // Check for null, meaning it is probably new
        foreach (var agent in agentState?.AgentList ?? new List<AgentDetails>())
        {
            await _sender.Send(
                new RegisterAgentEvent(
                    AgentFromDetailsToEntity.MapToNew(
                        agent,
                        agent.Id.IsNullOrEmpty() 
                            ? Guid.NewGuid().ToString()
                            : agent.Id
                    )
                ),
                cancellationToken
            );
        }
    }

    private string GetAgentFileName()
    {
        return Path.Combine(
            _serverInfo.AppDataPath,
            "Agent",
            "Agent.state.json"
        );
    }
}
