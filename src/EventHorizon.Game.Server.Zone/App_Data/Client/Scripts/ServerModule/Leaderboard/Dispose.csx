/*
data:
    active: bool
    observer: ObserverBase
    inputHandle: string
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
using EventHorizon.Game.Client.Engine.Gui.Scripting.Observers;
using EventHorizon.Game.Client.Engine.Input.Unregister;

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
        logger.LogInformation("LeaderBoard - Dispose Script");

        var layoutId = "GUI_LeaderBoard.json";
        var guiId = layoutId;

        UnRegisterObserver(
            services,
            data,
            "observer"
        );

        await services.Mediator.Send(
            new DisposeOfGuiCommand(
                guiId
            )
        );

        await UnRegisterInput(
            services,
            data
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

    private async Task UnRegisterInput(
        ScriptServices services,
        ScriptData data
    )
    {
        var inputHandle = data.Get<string>(
            "inputHandle"
        );

        if (!string.IsNullOrWhiteSpace(inputHandle))
        {
            await services.Mediator.Send(
                new UnregisterInputCommand(
                    inputHandle
                )
            );
        }
    }
}