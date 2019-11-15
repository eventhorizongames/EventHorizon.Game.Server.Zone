using System.Linq;
using System.IO;
using EventHorizon.Zone.Core.Events.DirectoryService;
using EventHorizon.Zone.Core.Events.FileService;
using EventHorizon.Zone.System.Agent.Events.Get;
using EventHorizon.Zone.System.Agent.Events.Register;
using EventHorizon.Zone.System.Agent.Save.Mapper;
using EventHorizon.Zone.System.Agent.Save.Model;
using Newtonsoft.Json;

// Check to see if Agent/Reload contains any files
var reloadDirectoryFiles = await Services.Mediator.Send(
    new GetListOfFilesFromDirectory(
        GetReloadAgentFileDirectory()
    )
);
var containsFiles = reloadDirectoryFiles.Count() > 0;

if (containsFiles)
{
    // UnRegister Only Agents that are not Global
    var agentList = await Services.Mediator.Send(
        new GetAgentListEvent
        {
            Query = agent => !agent.IsGlobal
        }
    );
    foreach (var agent in agentList)
    {
        await Services.Mediator.Send(
            new UnRegisterAgent(
                agent.AgentId
            )
        );
    }

    // Take all files found in the Reload Folder,
    //  then Register them into the Agent System.
    foreach (var file in reloadDirectoryFiles)
    {
        var text =  await Services.Mediator.Send(
            new ReadAllTextFromFile(
                file.FullName
            )
        );
        var agentState = JsonConvert.DeserializeObject<AgentSaveState>(
            text
        );
        foreach (var agent in agentState.AgentList)
        {
            await Services.Mediator.Send(
                new RegisterAgentEvent
                {
                    Agent = AgentFromDetailsToEntity.MapToNew(
                        agent,
                        Guid.NewGuid().ToString()
                    ),
                }
            );
        }
        // Remove the File from Agent/Reload Folder
        await Services.Mediator.Send(
            new DeleteFile(
                file.FullName
            )
        );
    }

}

private string GetReloadAgentFileDirectory()
{
    return Path.Combine(
        Services.ServerInfo.AppDataPath,
        "Agent",
        "Reload"
    );
}