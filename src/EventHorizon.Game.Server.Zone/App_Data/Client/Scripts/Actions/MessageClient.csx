/*
data:
    messageCode: string
    messageTemplate: string
    templateData: object (TODO: This might not work, might need to change the data to something that should)
    templateDataArray: Array<string> (proposed) Values to replace based on index in array
*/

using System.Threading.Tasks;
using EventHorizon.Game.Client.Engine.Scripting.Api;
using EventHorizon.Game.Client.Engine.Scripting.Data;
using EventHorizon.Game.Client.Engine.Scripting.Services;
using EventHorizon.Game.Server.Actions.Agent;
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

        var message = data.Get<string>("message");
        var messageTemplate = data.Get<string>("messageTemplate");
        // TODO: This might not work, TESTING!
        var templateDataArray = data.Get<string[]>("templateDataArray");

        await services.Mediator.Send(
            new ClientActionMessageFromCombatSystemEvent(
                message,
                services.Localize(
                    services.Localize(
                        messageTemplate,
                        templateDataArray
                    ),
                    templateDataArray
                ) 
            )
        );
    }
}
