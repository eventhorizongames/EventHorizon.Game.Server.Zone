/**
 * $services: {
 *   logger: ILogger;
 *   eventService: IEventService;
 *   commandService: ICommandService;
 * }
 * $data: {
 *  eventsToDispose: Array<{ name:string; handler: ()=>void; context: any; }>;
 * }
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

public class __SCRIPT__
    : IClientScript
{
    public string Id => "__SCRIPT__";

    public async Task Run(
        ScriptServices services,
        ScriptData data
    )
    {
        var layoutId = "GUI_BackToMenu.json";
        var guiId = layoutId;
        var logger = services.Logger<__SCRIPT__>();
        data.Set(
            "active",
            false
        );
        logger.LogInformation("Back To Menu - Initialize");

        Func<Task> BackToMainMenuHandler = async () =>
        {
            await services.Mediator.Send(
                new TriggerPageReloadCommand()
            );
        };

        Func<Task> OnChange = async () =>
        {
            if (data.Get<bool>("active"))
            {
                await services.Mediator.Send(
                    new DisposeOfGuiCommand(
                        guiId
                    )
                );
                data.Set(
                    "active",
                    false
                );
            }
            var result = await services.Mediator.Send(
                new CreateGuiCommand(
                    guiId,
                    layoutId,
                    new List<IGuiControlData>
                    {
                        new GuiControlDataModel
                        {
                            ControlId = "back_to_main_menu-button",
                            Options = new GuiControlOptionsModel
                            {
                                { "textKey", "account_BackToMainMenu" },
                                { "text", "Hello" },
                                { "onClick", BackToMainMenuHandler },
                            },
                        },
                    }
                )
            );

            if (result.Success)
            {
                await services.Mediator.Send(
                    new ActivateGuiCommand(
                        guiId
                    )
                );
                data.Set(
                    "active",
                    true
                );
            }
        };

        var observer = new __SCRIPT__Observer(
            services,
            data,
            layoutId,
            OnChange
        );

        System.Console.WriteLine("Register Observer");
        services.RegisterObserver(
            observer
        );

        data.Set(
            "observer",
            observer
        );

        await OnChange();
    }
}

public class __SCRIPT__Observer
    : GuiLayoutDataChangedEventObserver
{
    private readonly ScriptServices _scriptServices;
    private readonly ScriptData _scriptData;
    private readonly string _layoutId;
    private readonly Func<Task> _onChange;

    public __SCRIPT__Observer(
        ScriptServices services,
        ScriptData data,
        string layoutId,
        Func<Task> onChange
    )
    {
        _scriptServices = services;
        _scriptData = data;
        _layoutId = layoutId;
        _onChange = onChange;
    }

    public Task Handle(
        GuiLayoutDataChangedEvent args
    )
    {
        System.Console.WriteLine("HI from Script! " + args.Id);
        if (args.Id == _layoutId)
        {
            return _onChange();
        }
        return Task.CompletedTask;
    }
}
