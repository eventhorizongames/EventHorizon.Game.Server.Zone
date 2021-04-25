/// <summary>
/// </summary>

using EventHorizon.Zone.Core.Events.DirectoryService;
using EventHorizon.Zone.Core.Events.FileService;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Backup.Events;
using EventHorizon.Zone.System.Wizard.Model.Scripts;
using EventHorizon.Zone.System.Wizard.Events.Json.Merge;
using System.Text.Json;

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
        ServerScriptData scriptData
    )
    {
        var logger = services.Logger<__SCRIPT__>();
        logger.LogDebug("__SCRIPT__ - System Script");

        var serverInfo = services.ServerInfo;
        // Get the Wizard Data from the script data
        var wizardData = scriptData.Get<WizardData>("WizardData");
        // Load Map.state.json as Raw Json
        var mapJson = await services.Mediator.Send(
            new ReadAllTextFromFile(
                Path.Combine(
                    serverInfo.CoreMapPath,
                    "Map.state.json"
                )
            )
        );

        // Merge the JSON strings
        var mergedJsonResult = await services.Mediator.Send(
            new MergeWizardDataIntoJsonCommand(
                wizardData,
                mapJson
            )
        );
        if (!mergedJsonResult.Success)
        {
            return new WizardServerScriptResponse(
                false,
                mergedJsonResult.ErrorCode
            );
        }

        // Save Backup of Current Map Data
        await services.Mediator.Send(
            new CreateBackupOfFileContentCommand(
                serverInfo.AppDataPath.MakePathRelative(
                   serverInfo.CoreMapPath
                ).Split(
                    Path.DirectorySeparatorChar
                ),
                "Map.state.json",
                mapJson
            )
        );

        // Save the Merge JSON Map State
        await services.Mediator.Send(
            new WriteAllTextToFile(
                Path.Combine(
                    serverInfo.CoreMapPath,
                    "Map.state.json"
                ),
                mergedJsonResult.Result
            )
        );

        return new WizardServerScriptResponse(
            true,
            string.Empty
        );
    }

    private static readonly JsonValueKind[] primitiveTypes = new JsonValueKind[]{
        JsonValueKind.Number,
        JsonValueKind.True,
        JsonValueKind.False
    };

    public static Dictionary<string, string> GetDataProperties(
        Dictionary<string, string> data,
        JsonProperty jsonElement,
        string parentPropertyName = null
    )
    {
        var key = NormailzeForEditor(jsonElement.Name);
        if (parentPropertyName is not null)
        {
            key = $"{parentPropertyName}:{key}";
        }

        if (jsonElement.Value.ValueKind == JsonValueKind.String)
        {
            data[key] = jsonElement.Value.GetString();
        }
        else if (primitiveTypes.Any(a => a == jsonElement.Value.ValueKind))
        {
            data[key] = jsonElement.Value.GetRawText();
        }
        else if (jsonElement.Value.ValueKind == JsonValueKind.Object)
        {
            foreach (var childProperty in jsonElement.Value.EnumerateObject())
            {
                data = GetDataProperties(
                    data,
                    childProperty,
                    key
                );
            }
        }
        return data;
    }

    public static string NormailzeForEditor(
        string key
    )
    {
        return char.ToLower(key.First()) + key.Substring(1);
    }
}
