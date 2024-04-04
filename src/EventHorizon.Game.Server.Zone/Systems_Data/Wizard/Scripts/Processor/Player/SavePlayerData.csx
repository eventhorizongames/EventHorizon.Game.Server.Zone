/// <summary>
/// </summary>

using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.DirectoryService;
using EventHorizon.Zone.Core.Events.FileService;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Backup.Events;
using EventHorizon.Zone.System.Server.Scripts.Model;
using EventHorizon.Zone.System.Wizard.Events.Json.Merge;
using EventHorizon.Zone.System.Wizard.Model.Scripts;
using MediatR;
using Microsoft.Extensions.Logging;

public class __SCRIPT__ : ServerScript
{
    public string Id => "__SCRIPT__";
    public IEnumerable<string> Tags => new List<string> { "SYSTEM", "wizard-processor" };

    public async Task<ServerScriptResponse> Run(
        ServerScriptServices services,
        ServerScriptData scriptData
    )
    {
        var logger = services.Logger<__SCRIPT__>();
        logger.LogDebug("__SCRIPT__ - System Script");

        var serverInfo = services.ServerInfo;
        // Get the Wizard Data from the script data
        var wizardData = scriptData.Get<WizardData>("WizardData");
        // Load Player.config.json as Raw Json
        var playerConfigJson = await services.Mediator.Send(
            new ReadAllTextFromFile(Path.Combine(serverInfo.PlayerPath, "Player.config.json"))
        );

        // Merge the JSON strings
        var mergedJsonResult = await services.Mediator.Send(
            new MergeWizardDataIntoJsonCommand(wizardData, playerConfigJson)
        );
        if (!mergedJsonResult.Success)
        {
            return new WizardServerScriptResponse(false, mergedJsonResult.ErrorCode);
        }

        // return new WizardServerScriptResponse(true, string.Empty);

        // Save Backup of Current Player Config Data
        await services.Mediator.Send(
            new CreateBackupOfFileContentCommand(
                serverInfo
                    .AppDataPath.MakePathRelative(serverInfo.PlayerPath)
                    .Split(Path.DirectorySeparatorChar),
                "Player.config.json",
                playerConfigJson
            )
        );

        // Save the Merge JSON Player Config Data
        await services.Mediator.Send(
            new WriteAllTextToFile(
                Path.Combine(serverInfo.PlayerPath, "Player.config.json"),
                mergedJsonResult.Result
            )
        );

        return new WizardServerScriptResponse(true, string.Empty);
    }
}
