
using System.IO;
using EventHorizon.Zone.System.Agent.Events.Get;
using EventHorizon.Zone.System.Agent.Events.Register;
using EventHorizon.Zone.System.Agent.Save.Mapper;
using EventHorizon.Zone.System.Agent.Save.Model;
using Newtonsoft.Json;

// Check to see if Agent/Reload contains any files
var reloadDirectory = new DirectoryInfo(GetReloadAgentFileDirectory());
var containsFiles = reloadDirectory.GetFiles().Length > 0;

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
    var reloadFiles = reloadDirectory.GetFiles();
    foreach (var file in reloadFiles)
    {
        var text = File.ReadAllText(
            file.FullName
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
        file.Delete();
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