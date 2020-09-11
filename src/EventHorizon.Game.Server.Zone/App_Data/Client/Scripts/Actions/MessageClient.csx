/*
data:
    messageCode: string
    messageTemplate: string
    templateData: object (TODO: This might not work, might need to change the data to something that should)
    templateDataArray: Array<string> (proposed) Values to replace based on index in array
*/

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventHorizon.Game.Client.Engine.Scripting.Api;
using EventHorizon.Game.Client.Engine.Scripting.Data;
using EventHorizon.Game.Client.Engine.Scripting.Services;
using EventHorizon.Game.Server.ServerModule.CombatSystemLog.ClientAction.Message;
using Microsoft.Extensions.Logging;

// TODO: [Combat] - Finish with Implementation Combat System
public class __SCRIPT__
    : IClientScript
{
    public string Id => "__SCRIPT__";

    public async Task Run(
        ScriptServices services,
        ScriptData data
    )
    {
        var logger = services.Logger<__SCRIPT__>();
        logger.LogDebug("MessageClient - Script");

        logger.LogDebug("MessageClient - messageCode");
        var message = data.Get<string>("messageCode");
        logger.LogDebug("MessageClient - messageTemplate");
        var messageTemplate = data.Get<string>("messageTemplate");
        var templateData = data.Get<Dictionary<string, object>>("templateData");

        logger.LogDebug(
            "Message: {Message}",
            services.Localizer.Template(
                services.Localizer.Template(
                    messageTemplate,
                    templateData
                ),
                templateData
            )
        );

        await services.Mediator.Publish(
            new ClientActionMessageFromCombatSystemEvent(
                // TODO: This will not work, need a TemplateLocalize functionality
                // Has to support templates like: ${targetName}) is now owned by (${casterName}).
                services.Localizer.Template(
                    services.Localizer.Template(
                        messageTemplate,
                        templateData
                    ),
                    templateData
                )
            )
        );
    }
}
