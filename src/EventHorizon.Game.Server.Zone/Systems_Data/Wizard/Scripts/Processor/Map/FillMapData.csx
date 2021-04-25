/// <summary>
/// </summary>

using EventHorizon.Zone.Core.Events.DirectoryService;
using EventHorizon.Zone.Core.Events.FileService;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Wizard.Model.Scripts;
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
        // Load Map.state.json as Raw Json
        var data = new Dictionary<string, string>();
        var mapDocument = JsonDocument.Parse(
            await services.Mediator.Send(
                new ReadAllTextFromFile(
                    Path.Combine(
                        serverInfo.CoreMapPath,
                        "Map.state.json"
                    )
                )
            )
        );
        foreach (var item in mapDocument.RootElement.EnumerateObject())
        {
            data = GetDataProperties(
                data,
                item
            );
        }

        foreach (var item in data)
        {
            System.Console.WriteLine($"data[{item.Key}] = {item.Value}");
        }

        return new WizardServerScriptResponse(
            true,
            string.Empty,
            data
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
