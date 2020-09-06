/*
data:
*/

using System.Threading.Tasks;
using EventHorizon.Game.Client.Engine.Scripting.Api;
using EventHorizon.Game.Client.Engine.Scripting.Data;
using EventHorizon.Game.Client.Engine.Scripting.Services;
using EventHorizon.Game.Server.Actions.Agent;
using Microsoft.Extensions.Logging;

// TODO: [Combat] - Finish with Implementation Combat System
public class __SCRIPT__
    : IClientScript
{
    public string Id => "__SCRIPT__";

    public Task Run(
        ScriptServices services,
        ScriptData data
    )
    {
        var logger = services.Logger<__SCRIPT__>();
        logger.LogDebug("FreezeEntity - Script");

        // TODO: Lookup entity, and "Freeze" them

        return Task.CompletedTask;
    }
}
