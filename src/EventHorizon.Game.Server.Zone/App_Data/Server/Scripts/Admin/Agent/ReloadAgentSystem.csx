/// <summary>
/// This script will reload the Agent System.
/// 
/// Data: IDictionary<string, object>
/// - Command: <see cref="EventHorizon.Zone.System.Admin.Plugin.Command.Model.IAdminCommand" />
/// Services: <see cref="EventHorizon.Zone.System.Server.Scripts.Model.ServerScriptServices" />
/// </summary>

using System;
using System.Linq;
using System.IO;
using EventHorizon.Zone.Core.Events.DirectoryService;
using EventHorizon.Zone.Core.Events.FileService;
using EventHorizon.Zone.System.Agent.Events.Get;
using EventHorizon.Zone.System.Agent.Events.Register;
using EventHorizon.Zone.System.Agent.Save.Mapper;
using EventHorizon.Zone.System.Agent.Save.Model;
using Newtonsoft.Json;

using System.Collections.Generic;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Server.Scripts.Model;
using Microsoft.Extensions.Logging;
using EventHorizon.Zone.System.Admin.Plugin.Command.Model.Scripts;

public class __SCRIPT__
    : ServerScript
{
    public string Id => "__SCRIPT__";
    public IEnumerable<string> Tags => new List<string> { "testing-tag" };

    public async Task<ServerScriptResponse> Run(
        ServerScriptServices services,
        ServerScriptData data
    )
    {
        var logger = services.Logger<__SCRIPT__>();
        logger.LogDebug("__SCRIPT__ - Server Script");

        // Check to see if Agent/Reload contains any files
        var reloadDirectoryFiles = await services.Mediator.Send(
            new GetListOfFilesFromDirectory(
                GetReloadAgentFileDirectory(services)
            )
        );
        var containsFiles = reloadDirectoryFiles.Count() > 0;

        if (containsFiles)
        {
            // UnRegister Only Agents that are not Global
            var agentList = await services.Mediator.Send(
                new GetAgentListEvent
                {
                    Query = agent => !agent.IsGlobal
                }
            );
            foreach (var agent in agentList)
            {
                await services.Mediator.Send(
                    new UnRegisterAgent(
                        agent.AgentId
                    )
                );
            }

            // Take all files found in the Reload Folder,
            //  then Register them into the Agent System.
            foreach (var file in reloadDirectoryFiles)
            {
                var text = await services.Mediator.Send(
                    new ReadAllTextFromFile(
                        file.FullName
                    )
                );
                var agentState = JsonConvert.DeserializeObject<AgentSaveState>(
                    text
                );
                foreach (var agent in agentState.AgentList)
                {
                    await services.Mediator.Send(
                        new RegisterAgentEvent(
                            AgentFromDetailsToEntity.MapToNew(
                                agent,
                                Guid.NewGuid().ToString()
                            )
                        )
                    );
                }
                // Remove the File from Agent/Reload Folder
                await services.Mediator.Send(
                    new DeleteFile(
                        file.FullName
                    )
                );
            }

        }

        return new AdminCommandScriptResponse(
            true, // Success
            "agent_system_reloaded" // Message
        );
    }

    private string GetReloadAgentFileDirectory(
        ServerScriptServices services
    )
    {
        return Path.Combine(
            services.ServerInfo.AppDataPath,
            "Agent",
            "Reload"
        );
    }
}
