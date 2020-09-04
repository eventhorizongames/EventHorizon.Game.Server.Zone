/*
data:
    observer: ObserverBase
    active: bool
*/

using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using EventHorizon.Game.Client.Engine.Gui.Dispose;
using EventHorizon.Game.Client.Engine.Scripting.Services;
using EventHorizon.Game.Client.Engine.Scripting.Api;
using EventHorizon.Game.Client.Engine.Scripting.Data;
using Microsoft.Extensions.Logging;
using EventHorizon.Game.Client.Engine.Gui.Create;
using EventHorizon.Game.Client.Engine.Gui.Api;
using EventHorizon.Game.Client.Engine.Gui.Model;
using EventHorizon.Game.Client.Engine.Gui.Activate;
using EventHorizon.Game.Client.Engine.Gui.Changed;
using EventHorizon.Game.Server.ServerModule.BackToMenu.Reload;
using EventHorizon.Game.Client.Engine.Gui.Scripting.Observers;

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
        logger.LogDebug("Back To Menu - Initialize Script");

        var layoutId = "GUI_BackToMenu.json";
        var guiId = layoutId;

        Func<Task> BackToMainMenuHandler = async () =>
        {
            await services.Mediator.Send(
                new TriggerPageReloadCommand()
            );
        };

        var observer = new ScriptGuiLayoutDataChangedObserver(
            services,
            data,
            layoutId,
            guiId,
            () => new List<IGuiControlData>
            {
                new GuiControlDataModel
                {
                    ControlId = "back_to_main_menu-button",
                    Options = new GuiControlOptionsModel
                    {
                        { "onClick", BackToMainMenuHandler },
                    },
                },
            }
        );

        services.RegisterObserver(
            observer
        );

        data.Set(
            ScriptGuiLayoutDataChangedObserver.DataKey(
                layoutId,
                guiId,
                "observer"
            ),
            observer
        );

        await observer.OnChange();
    }
}
