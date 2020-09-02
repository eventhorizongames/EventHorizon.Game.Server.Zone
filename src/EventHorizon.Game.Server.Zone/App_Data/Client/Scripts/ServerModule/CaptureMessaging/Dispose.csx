/*
data:
    active: bool
    observer: ObserverBase
    messageObserver: ObserverBase
    timer: ITimerService
*/
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using EventHorizon.Game.Client.Engine.Gui.Dispose;
using Microsoft.Extensions.Logging;
using EventHorizon.Game.Client.Engine.Scripting.Api;
using EventHorizon.Game.Client.Engine.Scripting.Services;
using EventHorizon.Game.Client.Engine.Scripting.Data;
using EventHorizon.Observer.Model;
using EventHorizon.Game.Client.Core.Timer.Api;

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
        logger.LogInformation("Capture Messaging - Dispose Script");

        var guiId = "GUI_CaptureMessaging.json";

        UnRegisterObserver(
            services,
            data,
            "observer"
        );
        UnRegisterObserver(
            services,
            data,
            "messageObserver"
        );

        var timer = data.Get<ITimerService>(
            "timer"
        );
        if (timer != null)
        {
            timer.Clear();
        }

        await services.Mediator.Send(
            new DisposeOfGuiCommand(
                guiId
            )
        );
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