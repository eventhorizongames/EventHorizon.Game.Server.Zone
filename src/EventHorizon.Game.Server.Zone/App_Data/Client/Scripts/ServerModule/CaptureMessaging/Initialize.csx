/*
data:
    active: bool
    observer: ObserverBase
    messageObserver: ObserverBase
    timer: ITimerService
*/
using System;
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
using EventHorizon.Game.Client.Engine.Gui.Scripting.Observers;
using EventHorizon.Game.Server.ServerModule.CaptureMessaging.ClientAction.Show;
using EventHorizon.Game.Server.ServerModule.SystemLog.Message;
using EventHorizon.Game.Client.Engine.Gui.Update;
using EventHorizon.Game.Client.Core.Timer.Api;
using EventHorizon.Game.Client.Engine.Gui.Hide;
using EventHorizon.Game.Client.Engine.Gui.Show;

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
        logger.LogInformation("Capture Messaging - Initialize Script");

        var layoutId = "GUI_CaptureMessaging.json";
        var guiId = layoutId;

        var observer = new ScriptGuiLayoutDataChangedObserver(
            services,
            data,
            layoutId,
            guiId,
            () => new List<IGuiControlData>()
        );

        data.Set(
            "observer",
            observer
        );

        services.RegisterObserver(
            observer
        );

        await observer.OnChange();

        var messageObserver = new __SCRIPT__Observer(
            services,
            data,
            layoutId,
            guiId
        );
        data.Set(
            "messageObserver",
            messageObserver
        );
        services.RegisterObserver(
            messageObserver
        );
    }
}

public class __SCRIPT__Observer
    : ClientActionShowFiveSecondCaptureMessageEventObserver,
    ClientActionShowTenSecondCaptureMessageEventObserver
{
    private readonly ScriptServices _scriptServices;
    private readonly ScriptData _scriptData;
    private readonly string _layoutId;
    private readonly string _guiId;

    public __SCRIPT__Observer(
        ScriptServices services,
        ScriptData data,
        string layoutId,
        string guiId
    )
    {
        _scriptServices = services;
        _scriptData = data;
        _layoutId = layoutId;
        _guiId = guiId;
    }

    public Task Handle(
        ClientActionShowFiveSecondCaptureMessageEvent notification
    )
    {
        return ShowMessage(
            _scriptServices.Translate(
                "game:dontHaveTime"
            )
        );
    }

    public Task Handle(
        ClientActionShowTenSecondCaptureMessageEvent notification
    )
    {
        return ShowMessage(
            _scriptServices.Translate(
                "game:stillHaveTime"
            )
        );
    }

    private async Task ShowMessage(
        string message
    )
    {
        await _scriptServices.Mediator.Publish(
            new ClientActionMessageFromSystemEvent(
                message,
                new GuiControlOptionsModel
                {
                    { "color", "green" }
                },
                new GuiControlOptionsModel()
            )
        );

        await _scriptServices.Mediator.Send(
            new UpdateGuiControlCommand(
                _guiId,
                new GuiControlDataModel
                {
                    ControlId = "capture_messaging-text",
                    Options = new GuiControlOptionsModel
                    {
                        { "text", message }
                    }
                }
            )
        );

        await _scriptServices.Mediator.Send(
            new ShowGuiCommand(
                _layoutId
            )
        );

        var timer = _scriptData.Get<ITimerService>(
            "timer"
        );
        if (timer != null)
        {
            timer.Clear();
        }
        else
        {
            timer = _scriptServices.GetService<ITimerService>();
        }

        timer.SetTimer(
            3000,
            () =>
            {
                _scriptServices.Mediator.Send(
                    new HideGuiCommand(
                        _layoutId
                    )
                ).ConfigureAwait(false).GetAwaiter().GetResult();
            }
        );
        _scriptData.Set(
            "timer",
            timer
        );
    }
}
