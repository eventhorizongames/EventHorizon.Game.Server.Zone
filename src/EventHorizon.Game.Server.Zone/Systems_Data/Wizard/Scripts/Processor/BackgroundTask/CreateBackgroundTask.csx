/// <summary>
/// </summary>

using EventHorizon.Zone.Core.Events.DirectoryService;
using EventHorizon.Zone.Core.Events.FileService;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Wizard.Model.Scripts;

using System.Collections.Generic;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Server.Scripts.Model;
using Microsoft.Extensions.Logging;
using System.IO;
using MediatR;

public class __SCRIPT__
    : ServerScript
{
    public string Id => "__SCRIPT__";
    public IEnumerable<string> Tags => new List<string> { "SYSTEM", "wizard-processor" };

    public async Task<ServerScriptResponse> Run(
        ServerScriptServices services,
        ServerScriptData data
    )
    {
        var logger = services.Logger<__SCRIPT__>();
        logger.LogDebug("__SCRIPT__ - System Script");

        var serverInfo = services.ServerInfo;
        var wizardStep = data.Get<WizardStep>("WizardStep");
        var wizardData = data.Get<WizardData>("WizardData");
        var idPrefix = wizardStep.Details["IdPrefix"];
        var locationProperty = wizardStep.Details["LocationProperty"];
        var folder = wizardData["folderName"];
        var taskName = wizardData["taskName"];
        var taskPeriod = wizardData["taskPeriod"];

        var absoluteDirectory = Path.Combine(
            serverInfo.ServerScriptsPath,
            "Tasks",
            folder
        );
        await services.Mediator.Send(
            new CreateDirectory(
                absoluteDirectory
            )
        );

        // Create Files
        var fileToCreate = $"{taskName}BackgroundTask.csx";
        var contentFileFullName = Path.Combine(
            serverInfo.SystemsPath,
            "Wizard",
            "Templates",
            "BackgroundTask",
            "ScriptedBackgroundTask.csx.template"
        );
        var fileToCreateFullName = Path.Combine(
            absoluteDirectory,
            fileToCreate
        );
        if (await services.Mediator.Send(
            new DoesFileExist(
                fileToCreateFullName
            )
        ))
        {
            // Already exists, do not overwrite file.
            return new WizardServerScriptResponse(
                false,
                "wizard_validation_file_exist"
            );
        }

        await services.Mediator.Send(
            new WriteAllTextToFile(
                fileToCreateFullName,
                await CreateFile(
                    services.Mediator,
                    folder,
                    taskName,
                    contentFileFullName,
                    taskPeriod
                )
            )
        );

        // Set Location Property
        var scriptId = string.Join(
            "_",
            idPrefix, 
            "Tasks",
            folder,
            fileToCreate
        );
        var scriptEditFilePath = $"/edit/file/{scriptId.Base64Encode()}"; 
        var responseData = new Dictionary<string, string>();

        responseData[locationProperty] = scriptEditFilePath;

        return new WizardServerScriptResponse(
            true,
            string.Empty,
            responseData
        );
    }

    private async Task<string> CreateFile(
        IMediator mediator,
        string folder,
        string name,
        string contentFile,
        string taskPeriod
    )
    {
        var result = await mediator.Send(
            new ReadAllTextFromFile(
                contentFile
            )
        );
        return result.Replace(
            "[[NAME]]",
            $"{folder}_{name}"
        ).Replace(
            "[[TASK_PERIOD]]",
            taskPeriod
        );
    }
}
