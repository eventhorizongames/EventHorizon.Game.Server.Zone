/// <summary>
/// </summary>

using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Wizard.Model;
using EventHorizon.Zone.System.Wizard.Model.Scripts;
using EventHorizon.Zone.Core.Events.FileService;

using System.Collections.Generic;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Server.Scripts.Model;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;
using System.IO;

public class __SCRIPT__
    : ServerScript
{
    public string Id => "__SCRIPT__";
    public IEnumerable<string> Tags => new List<string> { "SYSTEM", "wizard-validator" };

    public async Task<ServerScriptResponse> Run(
        ServerScriptServices services,
        ServerScriptData data
    )
    {
        var logger = services.Logger<__SCRIPT__>();
        logger.LogDebug("__SCRIPT__ - System Script");

        var serverInfo = services.ServerInfo;
        var wizardData = data.Get<WizardData>("WizardData");
        logger.LogInformation("WizardData: {WizardData}", wizardData);

        // Validate Task Name is Exists/NotEmpty/Alphanumeric
        var taskName = string.Empty;
        if (!wizardData.TryGetData(
            "taskName",
            out taskName
        ) || string.IsNullOrWhiteSpace(taskName)
        || !isAlphaNumeric(taskName))
        {
            logger.LogInformation("Name: {WizardDataTaskName}", taskName);
            return new WizardServerScriptResponse(
                false,
                "wizard_validation_task_name_invalid"
            );
        }

        // Validate FolderName is Exists/NotEmpty/Alphanumeric
        var folder = string.Empty;
        if (!wizardData.TryGetData(
            "folderName",
            out folder
        ) || string.IsNullOrWhiteSpace(folder)
        || !isAlphaNumeric(folder))
        {
            return new WizardServerScriptResponse(
                false,
                "wizard_validation_folder_invalid"
            );
        }

        // Validate Task Period is greater than Zero
        if (!wizardData.TryGetData(
            "taskPeriods",
            out var taskPeriodString
        ) && int.TryParse(
            taskPeriodString,
            out var taskPeriod
        ) && taskPeriod > 0)
        {
            return new WizardServerScriptResponse(
                false,
                "wizard_validation_task_period_invalid"
            );
        } 

        // Validate File does not already exist
        var fileToCreate = $"{taskName}Backgroundtask.csx";
        var result = await services.Mediator.Send(
            new DoesFileExist(
                Path.Combine(
                    serverInfo.ServerScriptsPath,
                    "Tasks",
                    folder,
                    fileToCreate
                )
            )
        );
        if (result)
        {
            return new WizardServerScriptResponse(
                false,
                "wizard_validation_file_exist"
            );
        }

        return new WizardServerScriptResponse(
            true,
            string.Empty
        );
    }

    public static bool isAlphaNumeric(string strToCheck)
    {
        return new Regex(
            @"^[a-zA-Z0-9]*$"
        ).IsMatch(strToCheck);
    }

}
