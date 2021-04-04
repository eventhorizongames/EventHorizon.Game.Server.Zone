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
    public IEnumerable<string> Tags => new List<string> { "SYSTEM", "wizard-processor" };

    public async Task<ServerScriptResponse> Run(
        ServerScriptServices services,
        ServerScriptData data
    )
    {
        var logger = services.Logger<__SCRIPT__>();
        logger.LogDebug("__SCRIPT__ - System Script");

        var serverInfo = services.ServerInfo;
        var wizardData = data.Get<WizardData>("WizardData");
        var name = string.Empty;
        logger.LogInformation("WizardData: {WizardData}", wizardData);
        // Validate Name is Exists/NotEmpty/Alphanumeric
        if (!wizardData.TryGetValue(
            "Name",
            out name
        ) || string.IsNullOrWhiteSpace(name)
        || !isAlphaNumeric(name))
        {
            logger.LogInformation("Name: {WizardDataName}", name);
            return new WizardServerScriptResponse(
                false,
                "wizard_validation_name_invalid"
            );
        }

        var folder = string.Empty;
        // Validate Path is Exists/NotEmpty/Alphanumeric
        if (!wizardData.TryGetValue(
            "Folder",
            out folder
        ) || string.IsNullOrWhiteSpace(folder)
        || !isAlphaNumeric(folder))
        {
            return new WizardServerScriptResponse(
                false,
                "wizard_validation_folder_invalid"
            );
        }

        /*
         * 
            - Event Script
                <Name>Event
            - Event Observer Script
                <Name>EventObserver
            - Observer Handler Script
                Example<Name>EventObserverHandler
            - Observer Trigger Script
                Example<Name>EventTrigger
        */

        var filesToCreate = new List<string>
        {
            $"{name}Event.csx",
            $"{name}EventObserver.csx",
            $"Example{name}EventObserverHandler.csx",
            $"Example{name}EventTrigger.csx",
        };

        // Validate File(s) do not already exist
        foreach (var fileToCreate in filesToCreate)
        {
            var result = await services.Mediator.Send(
                new DoesFileExist(
                    Path.Combine(
                        serverInfo.ServerScriptsPath,
                        "Observers",
                        folder,
                        fileToCreate
                    )
                )
            );

            if (result)
            {
                return new WizardServerScriptResponse(
                    false,
                    "wizard_validation_files_exist"
                );
            }
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
