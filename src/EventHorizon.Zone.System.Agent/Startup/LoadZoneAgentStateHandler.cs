namespace EventHorizon.Zone.System.Agent.Startup
{
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.Core.Model.Json;
    using EventHorizon.Zone.Core.Model.Settings;
    using EventHorizon.Zone.System.Agent.Connection;
    using EventHorizon.Zone.System.Agent.Connection.Model;
    using EventHorizon.Zone.System.Agent.Events.Register;
    using EventHorizon.Zone.System.Agent.Events.Startup;
    using EventHorizon.Zone.System.Agent.Save.Mapper;
    using EventHorizon.Zone.System.Agent.Save.Model;

    using global::System;
    using global::System.Collections.Generic;
    using global::System.IO;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    public class LoadZoneAgentStateHandler : IRequestHandler<LoadZoneAgentStateEvent, Unit>
    {
        readonly IMediator _mediator;
        readonly ServerInfo _serverInfo;
        readonly ZoneSettings _zoneSettings;
        readonly IAgentConnection _agentConnection;
        readonly IJsonFileLoader _fileLoader;

        public LoadZoneAgentStateHandler(
            IMediator mediator,
            ServerInfo serverInfo,
            ZoneSettings zoneSettings,
            IAgentConnection agentConnection,
            IJsonFileLoader fileLoader
        )
        {
            _mediator = mediator;
            _serverInfo = serverInfo;
            _zoneSettings = zoneSettings;
            _agentConnection = agentConnection;
            _fileLoader = fileLoader;
        }
        public async Task<Unit> Handle(
            LoadZoneAgentStateEvent request,
            CancellationToken cancellationToken
        )
        {
            // Load "GLOBAL" Agents from Agent service.
            foreach (var agent in await _agentConnection.GetAgentList(
                _zoneSettings.Tag
            ))
            {
                await _mediator.Send(
                    new RegisterAgentEvent(
                        AgentFromDetailsToEntity.MapToNewGlobal(
                            agent
                        )
                    )
                );
            }
            // Load "LOCAL" agents from file service
            var agentState = await _fileLoader.GetFile<AgentSaveState>(
                GetAgentFileName()
            );

            // Check for null, meaning it is probably new
            foreach (var agent in agentState?.AgentList ?? new List<AgentDetails>())
            {
                await _mediator.Send(
                    new RegisterAgentEvent(
                        AgentFromDetailsToEntity.MapToNew(
                            agent,
                            agent.Id ?? Guid.NewGuid().ToString()
                        )
                    )
                );
            }

            return Unit.Value;
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
}
