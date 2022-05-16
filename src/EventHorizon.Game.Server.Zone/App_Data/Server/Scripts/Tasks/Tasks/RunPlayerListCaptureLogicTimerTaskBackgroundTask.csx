using System.Collections.Generic;
using System.Threading.Tasks;
using ScriptsPluginBackgroundTaskModel = EventHorizon.Zone.System.Server.Scripts.Plugin.BackgroundTask.Model;
using ServerScriptsModel = EventHorizon.Zone.System.Server.Scripts.Model;

// Tasks_RunPlayerListCaptureLogicTimerTaskBackgroundTask
public class __SCRIPT__ : ScriptsPluginBackgroundTaskModel.ScriptedBackgroundTask
{
    #region ServerScript Properties
    public string Id => "__SCRIPT__";
    public IEnumerable<string> Tags => new List<string> { "background-task" };
    #endregion

    #region BackgroundTask Properties
    public string TaskId => Id;

    // Is in milliseconds
    public int TaskPeriod { get; } = 1000;
    public IEnumerable<string> TaskTags => Tags;
    #endregion

    public async Task<ServerScriptsModel.ServerScriptResponse> Run(
        ServerScriptsModel.ServerScriptServices services,
        ServerScriptsModel.ServerScriptData data
    )
    {
        var logger = services.Logger<__SCRIPT__>();
        logger.LogDebug("__SCRIPT__ - Background Task Startup");

        return new ServerScriptsModel.StandardServerScriptResponse(
            true,
            "background_task_Tasks_RunPlayerListCaptureLogicTimerTask_started"
        );
    }

    public async Task TaskTrigger(ServerScriptsModel.ServerScriptServices services)
    {
        var logger = services.Logger<__SCRIPT__>();
        logger.LogDebug("__SCRIPT__ - Background Task Trigger");
        await services.ObserverBroker.Trigger(new RunCaptureLogicForAllPlayersEvent());
    }
}
