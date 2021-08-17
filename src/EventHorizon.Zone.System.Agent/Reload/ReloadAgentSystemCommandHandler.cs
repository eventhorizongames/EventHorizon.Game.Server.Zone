namespace EventHorizon.Zone.System.Agent.Reload
{
    using EventHorizon.Zone.Core.Events.DirectoryService;
    using EventHorizon.Zone.Core.Events.FileService;
    using EventHorizon.Zone.Core.Model.Command;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.System.Agent.Events.Get;
    using EventHorizon.Zone.System.Agent.Events.Register;
    using EventHorizon.Zone.System.Agent.Save.Mapper;
    using EventHorizon.Zone.System.Agent.Save.Model;

    using global::System;
    using global::System.IO;
    using global::System.Linq;
    using global::System.Text.Json;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    public class ReloadAgentSystemCommandHandler
        : IRequestHandler<ReloadAgentSystemCommand, StandardCommandResult>
    {
        private readonly IMediator _mediator;
        private readonly ServerInfo _serverInfo;

        public ReloadAgentSystemCommandHandler(
            IMediator mediator,
            ServerInfo serverInfo
        )
        {
            _mediator = mediator;
            _serverInfo = serverInfo;
        }

        public async Task<StandardCommandResult> Handle(
            ReloadAgentSystemCommand request,
            CancellationToken cancellationToken
        )
        {
            // Check to see if Agent/Reload contains any files
            var reloadDirectoryFiles = await _mediator.Send(
                new GetListOfFilesFromDirectory(
                    GetReloadAgentFileDirectory()
                )
            );
            var containsFiles = reloadDirectoryFiles.Count() > 0;

            if (containsFiles)
            {
                // UnRegister Only Agents that are not Global
                var agentList = await _mediator.Send(
                    new GetAgentListEvent(
                        agent => !agent.IsGlobal
                    )
                );
                foreach (var agent in agentList)
                {
                    await _mediator.Send(
                        new UnRegisterAgent(
                            agent.AgentId
                        )
                    );
                }

                // Take all files found in the Reload Folder,
                //  then Register them into the Agent System.
                foreach (var file in reloadDirectoryFiles)
                {
                    var text = await _mediator.Send(
                        new ReadAllTextFromFile(
                            file.FullName
                        )
                    );
                    var agentState = JsonSerializer.Deserialize<AgentSaveState>(
                        text
                    );
                    if (agentState.IsNotNull())
                    {
                        foreach (var agent in agentState.AgentList)
                        {
                            await _mediator.Send(
                                new RegisterAgentEvent(
                                    AgentFromDetailsToEntity.MapToNew(
                                        agent,
                                        Guid.NewGuid().ToString()
                                    )
                                )
                            );
                        }
                    }
                    // Remove the File from Agent/Reload Folder
                    await _mediator.Send(
                        new DeleteFile(
                            file.FullName
                        )
                    );
                }
                return new StandardCommandResult();
            }

            return new StandardCommandResult(
                "no_agents_to_load"
            );
        }

        private string GetReloadAgentFileDirectory()
        {
            return Path.Combine(
                _serverInfo.AppDataPath,
                "Agent",
                "Reload"
            );
        }
    }
}
