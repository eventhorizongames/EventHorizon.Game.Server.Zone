/*
data:
    active: bool
    observer: ObserverBase
    companionsCaught: IList<string>
    currentDisplayedCaught: IList<string>
*/

using System.Collections;
using System.Threading.Tasks;
using EventHorizon.Game.Client.Engine.Gui.Dispose;
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
        logger.LogDebug("PlayerCaptures - Dispose Script");

        var layoutId = "GUI_PlayerCaptures.json";
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