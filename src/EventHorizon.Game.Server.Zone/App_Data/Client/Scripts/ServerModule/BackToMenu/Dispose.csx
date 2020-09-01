/**
 * $services: {
 *   logger: ILogger;
 *   eventService: IEventService;
 *   commandService: ICommandService;
 * }
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
        logger.LogDebug("Back To Menu - Dispose");
        
        var guiId = "GUI_BackToMenu.json";
        var observer = data.Get<ObserverBase>(
            "observer"
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