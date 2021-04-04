/// <summary>
/// </summary>

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
        var wizardData = data.Get<WizardData>("WizardData");
        var folder = wizardData["Folder"];
        var name = wizardData["Name"];

        var filesToCreate = new List<(string fileToCreate, string contentFile)>
        {
            ($"{name}Event.csx", "Event.csx.template"),
            ($"{name}EventObserver.csx", "EventObserver.csx.template"),
            ($"Example{name}EventObserverHandler.csx", "ExampleEventObserverHandler.csx.template"),
            ($"Example{name}EventTrigger.csx", "ExampleEventTrigger.csx.template"),
        };

        var absoluteDirectory = Path.Combine(
            serverInfo.ServerScriptsPath,
            "Observers",
            folder
        );
        await services.Mediator.Send(
            new CreateDirectory(
                absoluteDirectory
            )
        );

        // Create Files
        foreach (var (fileToCreate, contentFileName) in filesToCreate)
        {
            var contentFileFullName = Path.Combine(
                serverInfo.SystemsPath,
                "Wizard",
                "Templates",
                "Observer",
                contentFileName
            );
            var fileToCreateFullName = Path.Combine(
                absoluteDirectory,
                fileToCreate
            );
            if(await services.Mediator.Send(
                new DoesFileExist(
                    fileToCreateFullName
                )
            ))
            {
                // Already exists, do not overwrite file.
                continue;
            }
            await services.Mediator.Send(
                new WriteAllTextToFile(
                    fileToCreateFullName,
                    await CreateFile(
                        services.Mediator,
                        folder,
                        name,
                        contentFileFullName
                    )
                )
            );
        }

        return new WizardServerScriptResponse(
            true,
            string.Empty
        );
    }

    private async Task<string> CreateFile(
        IMediator mediator,
        string folder,
        string name,
        string contentFile
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
        );
    }
}
