/*
data:
    observer: ObserverBase
    active: bool
*/

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
        logger.LogDebug("Back To Menu - Dispose Script");
        
        var layoutId = "GUI_BackToMenu.json";
        var guiId = layoutId;
        var observer = data.Get<ObserverBase>(
            ScriptGuiLayoutDataChangedObserver.DataKey(
                layoutId,
                guiId,
                "observer"
            )
        );

        if (observer != null)
        {
            services.UnRegisterObserver(
                observer
            );
        }
        await services.Mediator.Send(
            new DisposeOfGuiCommand(
                guiId
            )
        );
    }
}