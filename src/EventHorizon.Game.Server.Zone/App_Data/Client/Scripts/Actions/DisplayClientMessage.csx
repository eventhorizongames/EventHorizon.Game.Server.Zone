/*
data:
    messageKey: string
    messageData: IDictionary<string, object>
*/

using System.Collections.Generic;
using System.Threading.Tasks;
using EventHorizon.Game.Client.Engine.Scripting.Api;
using EventHorizon.Game.Client.Engine.Scripting.Data;
using EventHorizon.Game.Client.Engine.Scripting.Services;
using EventHorizon.Game.Server.ServerModule.FeedbackMessage.Display;
using Microsoft.Extensions.Logging;

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
        logger.LogDebug("__SCRIPT__ Script");

        // TODO: [TESTING] - Check for Feeback messages from Game related logic

        await services.Mediator.Publish(
            new DisplayFeedbackMessageEvent(
                services.Localizer.Template(
                    data.Get<string>("messageKey"),
                    data.Get<Dictionary<string, object>>("messageData")
                )
            )
        );
    }
}
