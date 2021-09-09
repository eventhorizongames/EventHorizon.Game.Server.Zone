using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.Core.Model.Player;
using EventHorizon.Zone.System.Server.Scripts.Model;
using EventHorizon.Observer.Model;

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

using EventHorizon.Zone.System.Server.Scripts.Plugin.BackgroundTask.Model;
using System;

public class __SCRIPT__
    : ScriptedBackgroundTask
{
    #region ServerScript Properties
    public string Id => "__SCRIPT__";
    public IEnumerable<string> Tags => new List<string> { "background-task" };
    #endregion

    #region BackgroundTask Properties
    public string TaskId => Id;
    // in ms
    public int TaskPeriod { get; } = 10000;
    public IEnumerable<string> TaskTags => Tags;
    #endregion

    public async Task<ServerScriptResponse> Run(
        ServerScriptServices services,
        ServerScriptData data
    )
    {
        var logger = services.Logger<__SCRIPT__>();
        logger.LogDebug("__SCRIPT__ - Background Task Startup");

        return new StandardServerScriptResponse(
            true,
            "background_task_ScriptedCountDown_started"
        );
    }

    public async Task TaskTrigger(
        ServerScriptServices services
    )
    {
        var logger = services.Logger<__SCRIPT__>();
        logger.LogDebug("__SCRIPT__ - Background Task Trigger");
    }
}
