/*
data:
*/

using System.Threading.Tasks;
using EventHorizon.Game.Client.Engine.Scripting.Api;
using EventHorizon.Game.Client.Engine.Scripting.Data;
using EventHorizon.Game.Client.Engine.Scripting.Services;
using Microsoft.Extensions.Logging;

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
        logger.LogDebug("__SCRIPT__ - Script");

        return Task.CompletedTask;
    }
}
