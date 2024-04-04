/// <summary>
/// </summary>

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.DirectoryService;
using EventHorizon.Zone.Core.Events.FileService;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Server.Scripts.Model;
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

        // Load Player.config.json as Raw Json
        var playerConfigFileText = await services.Mediator.Send(
            new ReadAllTextFromFile(Path.Combine(serverInfo.PlayerPath, "Player.config.json"))
        );
        var playerConfig = JsonDocument.Parse(playerConfigFileText);
        var data = services.DataParsers.FlattenJsonDocumentParser(playerConfig);

        foreach (var item in data)
        {
            System.Console.WriteLine($"data[{item.Key}] = {item.Value}");
        }

        return new WizardServerScriptResponse(true, string.Empty, data);
    }
}
