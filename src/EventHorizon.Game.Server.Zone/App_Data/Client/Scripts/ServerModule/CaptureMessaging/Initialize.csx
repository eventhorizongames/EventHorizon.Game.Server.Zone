using System.Collections.Generic;
using System.Threading.Tasks;
using FactoryApi = EventHorizon.Game.Client.Core.Factory.Api;
using GuiApi = EventHorizon.Game.Client.Engine.Gui.Api;
using GuiHide = EventHorizon.Game.Client.Engine.Gui.Hide;
using GuiModel = EventHorizon.Game.Client.Engine.Gui.Model;
using GuiScripting = EventHorizon.Game.Client.Engine.Gui.Scripting.Observers;
using GuiShow = EventHorizon.Game.Client.Engine.Gui.Show;
using GuiUpdate = EventHorizon.Game.Client.Engine.Gui.Update;
using ScriptingApi = EventHorizon.Game.Client.Engine.Scripting.Api;
using ScriptingData = EventHorizon.Game.Client.Engine.Scripting.Data;
using ScriptingServices = EventHorizon.Game.Client.Engine.Scripting.Services;
using SystemLogMessage = EventHorizon.Game.Server.ServerModule.SystemLog.Message;
using TimerApi = EventHorizon.Game.Client.Core.Timer.Api;

public class __SCRIPT__ : ScriptingApi.IClientScript
{
    public string Id => "__SCRIPT__";

    public async Task Run(ScriptingServices.ScriptServices services, ScriptingData.ScriptData data)
    {
        var logger = services.Logger<__SCRIPT__>();
        logger.LogDebug("Capture Messaging - Initialize Script");
        logger.LogDebug("Capture Messaging - Initialize Script");

        var layoutId = "GUI_CaptureMessaging.json";
        var guiId = layoutId;

        var observer = new GuiScripting.ScriptGuiLayoutDataChangedObserver(
            services,
            data,
            layoutId,
            guiId,
            () => new List<GuiApi.IGuiControlData>()
        );

        data.Set(
            GuiScripting.ScriptGuiLayoutDataChangedObserver.DataKey(layoutId, guiId, "observer"),
            observer
        );

        services.RegisterObserver(observer);

        await observer.OnChange();

        var messageObserver = new __SCRIPT__Observer(services, data, layoutId, guiId);
        data.Set("messageObserver", messageObserver);
        services.RegisterObserver(messageObserver);
    }
}

public class __SCRIPT__Observer
    : Game_ClientActions_ClientActionShowFiveSecondCaptureMessageEventObserver,
      Game_ClientActions_ClientActionShowTenSecondCaptureMessageEventObserver
{
    private readonly ScriptingServices.ScriptServices _scriptServices;
    private readonly ScriptingData.ScriptData _scriptData;
    private readonly string _layoutId;
    private readonly string _guiId;

    public __SCRIPT__Observer(
        ScriptingServices.ScriptServices services,
        ScriptingData.ScriptData data,
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
        Game_ClientActions_ClientActionShowFiveSecondCaptureMessageEvent notification
    )
    {
        return ShowMessage(_scriptServices.Localizer["game:dontHaveTime"]);
    }

    public Task Handle(Game_ClientActions_ClientActionShowTenSecondCaptureMessageEvent notification)
    {
        return ShowMessage(_scriptServices.Localizer["game:stillHaveTime"]);
    }

    private async Task ShowMessage(string message)
    {
        await _scriptServices.Mediator.Publish(
            new SystemLogMessage.ClientActionMessageFromSystemEvent(
                message,
                new GuiModel.GuiControlOptionsModel { { "color", "green" } },
                new GuiModel.GuiControlOptionsModel()
            )
        );

        await _scriptServices.Mediator.Send(
            new GuiUpdate.UpdateGuiControlCommand(
                _guiId,
                new GuiModel.GuiControlDataModel
                {
                    ControlId = "capture_messaging-text",
                    Options = new GuiModel.GuiControlOptionsModel { { "text", message } }
                }
            )
        );

        await _scriptServices.Mediator.Send(new GuiShow.ShowGuiCommand(_layoutId));

        var timer = _scriptData.Get<TimerApi.ITimerService>("timer");
        if (timer != null)
        {
            timer.Clear();
        }
        else
        {
            timer = _scriptServices
                .GetService<FactoryApi.IFactory<TimerApi.ITimerService>>()
                .Create();
        }

        timer.SetTimer(
            3000,
            () =>
            {
                _scriptServices.Mediator
                    .Send(new GuiHide.HideGuiCommand(_layoutId))
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();
            }
        );
        _scriptData.Set("timer", timer);
    }
}
