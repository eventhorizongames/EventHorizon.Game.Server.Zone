/*
data:
    active: bool
    observer: ObserverBase
    messageObserver: ObserverBase
    timer: ITimerService
*/

using System.Collections.Generic;
using System.Threading.Tasks;
using EventHorizon.Game.Client.Engine.Gui.Dispose;
using EventHorizon.Game.Client.Engine.Gui.Scripting.Observers;
using EventHorizon.Game.Client.Engine.Scripting.Api;
using EventHorizon.Game.Client.Engine.Scripting.Data;
using EventHorizon.Game.Client.Engine.Scripting.Services;
using EventHorizon.Observer.Model;
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
        logger.LogDebug("Gui - Dispose Script");


        var layoutList = data.Get<List<string>>(
            "layoutList"
        ) ?? new List<string>();

        foreach (var layoutId in layoutList)
        {
            var guiId = layoutId;

            UnRegisterObserver(
                services,
                data,
                ScriptGuiLayoutDataChangedObserver.DataKey(
                    layoutId,
                    guiId,
                    "observer"
                )
            );

            await services.Mediator.Send(
                new DisposeOfGuiCommand(
                    guiId
                )
            );
        }
    }

    private void UnRegisterObserver(
        ScriptServices services,
        ScriptData data,
        string observerName
    )
    {
        var observer = data.Get<ObserverBase>(
            observerName
        );

        if (observer != null)
        {
            services.UnRegisterObserver(
                observer
            );
        }
    }
}