/*
data:
    active: bool
    observer: ObserverBase
    show: bool
    messageCount: int
*/

using System.Collections.Generic;
using System.Threading.Tasks;
using EventHorizon.Game.Client.Engine.Gui.Activate;
using EventHorizon.Game.Client.Engine.Gui.Changed;
using EventHorizon.Game.Client.Engine.Gui.Create;
using EventHorizon.Game.Client.Engine.Gui.Dispose;
using EventHorizon.Game.Client.Engine.Gui.Generate;
using EventHorizon.Game.Client.Engine.Gui.Hide;
using EventHorizon.Game.Client.Engine.Gui.Model;
using EventHorizon.Game.Client.Engine.Gui.Show;
using EventHorizon.Game.Client.Engine.Scripting.Api;
using EventHorizon.Game.Client.Engine.Scripting.Data;
using EventHorizon.Game.Client.Engine.Scripting.Services;
using EventHorizon.Game.Server.ServerModule.SystemLog.Hide;
using EventHorizon.Game.Server.ServerModule.SystemLog.Message;
using EventHorizon.Game.Server.ServerModule.SystemLog.Show;
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
        logger.LogDebug("SystemLog - Initialize Script");

        var layoutId = "GUI_System_Log.json";
        var guiId = layoutId;
        var observer = new __SCRIPT__Observer(
            services,
            data,
            layoutId,
            guiId
        );

        data.Set(
            "observer",
            observer
        );

        services.RegisterObserver(
            observer
        );

        await observer.OnChange();
    }
}

public class __SCRIPT__Observer
    : GuiLayoutDataChangedEventObserver,
    ClientActionMessageFromSystemEventObserver,
    ShowMessageFromSystemEventObserver,
    HideMessageFromSystemEventObserver
{
    private readonly ScriptServices _services;
    private readonly ScriptData _data;
    private readonly string _layoutId;
    private readonly string _guiId;

    private readonly string _messageLayoutId = "GUI_System_Log-Message.json";

    public __SCRIPT__Observer(
        ScriptServices services,
        ScriptData data,
        string layoutId,
        string guiId
    )
    {
        _services = services;
        _data = data;
        _layoutId = layoutId;
        _guiId = guiId;

        _data.Set(
            "show",
            false
        );
        _data.Set(
            "messageCount",
            0
        );
    }

    public async Task Handle(
        GuiLayoutDataChangedEvent args
    )
    {
        if (args.Id == _layoutId)
        {
            await OnChange();
        }
    }

    public Task Handle(
        ShowMessageFromSystemEvent args
    )
    {
        _data.Set(
            "show",
            true
        );
        return _services.Mediator.Send(
            new ShowGuiCommand(
                _guiId
            )
        );
    }

    public Task Handle(
        HideMessageFromSystemEvent args
    )
    {
        _data.Set(
            "show",
            false
        );
        return _services.Mediator.Send(
            new HideGuiCommand(
                _guiId
            )
        );
    }

    public async Task Handle(
        ClientActionMessageFromSystemEvent args
    )
    {
        var messageCount = _data.Get<int>(
            "messageCount"
        );
        messageCount++;
        _data.Set(
            "messageCount",
            messageCount
        );
        if (string.IsNullOrWhiteSpace(
            args.Message
        ))
        {
            return;
        }

        var newMessageGuiId = $"GUI_System_Log-Message_{messageCount}";
        var parentControlId = await _services.Mediator.Send(
            new GetGeneratedGuiControlId(
                _guiId,
                "gui_system_log-panel"
            )
        );
        await _services.Mediator.Send(
            new CreateGuiCommand(
                newMessageGuiId,
                _messageLayoutId,
                parentControlId: parentControlId,
                controlDataList: new List<GuiControlDataModel>
                {
                    new GuiControlDataModel
                    {
                        ControlId = "gui_system_message-sender",
                        Options = new GuiControlOptionsModel(
                            args.SenderControlOptions
                        ),
                    },
                    new GuiControlDataModel
                    {
                        ControlId = "gui_system_message-message",
                        Options = new GuiControlOptionsModel(
                            args.MessageControlOptions
                        )
                        {
                            { "text", args.Message }
                        },
                    },
                }
            )
        );

        await _services.Mediator.Send(
            new ActivateGuiCommand(
                newMessageGuiId
            )
        );
    }

    public async Task OnChange()
    {
        if (_data.Get<bool>("active"))
        {
            await _services.Mediator.Send(
                new DisposeOfGuiCommand(
                    _guiId
                )
            );
            _data.Set(
                "active",
                false
            );
        }
        var result = await _services.Mediator.Send(
            new CreateGuiCommand(
                _guiId,
                _layoutId
            )
        );

        if (result.Success)
        {
            await _services.Mediator.Send(
                new ActivateGuiCommand(
                    _guiId
                )
            );
            _data.Set(
                "active",
                true
            );
            if (_data.Get<bool>("show"))
            {
                await _services.Mediator.Send(
                    new ShowGuiCommand(
                        _guiId
                    )
                );
            }
        }
    }
}
