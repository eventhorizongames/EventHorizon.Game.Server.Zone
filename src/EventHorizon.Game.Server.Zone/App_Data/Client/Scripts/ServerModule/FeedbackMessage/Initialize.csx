/*
data:
    active: bool
    observer: ObserverBase
    messageObserver: ObserverBase
    timer: ITimerService
*/

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventHorizon.Game.Client.Core.Factory.Api;
using EventHorizon.Game.Client.Core.Timer.Api;
using EventHorizon.Game.Client.Engine.Gui.Api;
using EventHorizon.Game.Client.Engine.Gui.Hide;
using EventHorizon.Game.Client.Engine.Gui.Model;
using EventHorizon.Game.Client.Engine.Gui.Scripting.Observers;
using EventHorizon.Game.Client.Engine.Gui.Show;
using EventHorizon.Game.Client.Engine.Gui.Update;
using EventHorizon.Game.Client.Engine.Scripting.Api;
using EventHorizon.Game.Client.Engine.Scripting.Data;
using EventHorizon.Game.Client.Engine.Scripting.Services;
using EventHorizon.Game.Server.ServerModule.FeedbackMessage.Display;
using EventHorizon.Game.Server.ServerModule.SystemLog.Message;
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
        logger.LogDebug("Feedback Message - Initialize Script");

        var layoutId = "GUI_FeedbackMessage.json";
        var guiId = layoutId;

        var observer = new ScriptGuiLayoutDataChangedObserver(
            services,
            data,
            layoutId,
            guiId,
            () => new List<IGuiControlData>()
        );

        data.Set(
            ScriptGuiLayoutDataChangedObserver.DataKey(
                layoutId,
                guiId,
                "observer"
            ),
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
    : DisplayFeedbackMessageEventObserver
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

    public async Task Handle(
        DisplayFeedbackMessageEvent notification
    )
    {
        await _scriptServices.Mediator.Publish(
            new ClientActionMessageFromSystemEvent(
                notification.Message,
                new GuiControlOptionsModel(),
                new GuiControlOptionsModel
                {
                    { "color", "red" }
                }
            )
        );

        await _scriptServices.Mediator.Send(
            new UpdateGuiControlCommand(
                _guiId,
                new GuiControlDataModel
                {
                    ControlId = "feedback_message-text",
                    Options = new GuiControlOptionsModel
                    {
                        { "color", "red" },
                        { "text", notification.Message },
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
            timer = _scriptServices.GetService<IFactory<ITimerService>>().Create();
        }

        timer.SetTimer(
            5000,
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
