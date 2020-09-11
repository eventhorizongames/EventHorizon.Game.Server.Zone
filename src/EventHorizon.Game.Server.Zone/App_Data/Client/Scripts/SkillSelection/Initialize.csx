/*
data:
    active: bool
    observer: ObserverBase
    skillList: IList<SkillDetails>
*/

using System.Collections.Generic;
using System.Threading.Tasks;
using EventHorizon.Game.Client.Engine.Gui.Activate;
using EventHorizon.Game.Client.Engine.Gui.Api;
using EventHorizon.Game.Client.Engine.Gui.Changed;
using EventHorizon.Game.Client.Engine.Gui.Create;
using EventHorizon.Game.Client.Engine.Gui.Dispose;
using EventHorizon.Game.Client.Engine.Gui.Generate;
using EventHorizon.Game.Client.Engine.Gui.Hide;
using EventHorizon.Game.Client.Engine.Gui.Model;
using EventHorizon.Game.Client.Engine.Gui.Register;
using EventHorizon.Game.Client.Engine.Gui.Show;
using EventHorizon.Game.Client.Engine.Scripting.Api;
using EventHorizon.Game.Client.Engine.Scripting.Data;
using EventHorizon.Game.Client.Engine.Scripting.Services;
using EventHorizon.Game.Server.ServerModule.SystemLog.Hide;
using EventHorizon.Game.Server.ServerModule.SystemLog.Message;
using EventHorizon.Game.Server.ServerModule.SystemLog.Show;
using EventHorizon.Game.Server.SkillSelection.Activated;
using EventHorizon.Game.Server.SkillSelection.Model;
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
        logger.LogDebug("SkillSelection - Initialize Script");

        var layoutId = "GUI_Module_SkillSelection.json";
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
    SkillSelectionGuiActivatedEventObserver
{
    private readonly ScriptServices _services;
    private readonly ScriptData _data;
    private readonly string _layoutId;
    private readonly string _guiId;

    private readonly string _skillListLayoutId = "GUI_Module_SkillSelection.SkillList";

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
    public async Task Handle(
        SkillSelectionGuiActivatedEvent args
    )
    {
        await OnChange();
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
            await _services.Mediator.Send(
                new ShowGuiCommand(
                    _guiId
                )
            );
            await CreateSkillSelectionGui();
        }
    }

    private async Task CreateSkillSelectionGui()
    {
        var skillListGuiId = _skillListLayoutId;
        var skillLayoutControlList = BuildSkillLayoutControlData();
        var _skillListLayoutData = new GuiLayoutDataModel
        {
            Id = _skillListLayoutId,
            Sort = 0,
            ControlList = skillLayoutControlList
        };

        await _services.Mediator.Send(
            new RegisterGuiLayoutDataCommand(
                _skillListLayoutData
            )
        );

        await _services.Mediator.Send(
            new CreateGuiCommand(
                skillListGuiId,
                _skillListLayoutId,
                parentControlId: await _services.Mediator.Send(
                    new GetGeneratedGuiControlId(
                        _guiId,
                        "onscreen_skill_selection-panel"
                    )
                )
            )
        );
        await _services.Mediator.Send(
            new ActivateGuiCommand(
                skillListGuiId
            )
        );
    }

    private List<GuiLayoutControlDataModel> BuildSkillLayoutControlData()
    {
        var result = new List<GuiLayoutControlDataModel>();
        var skillList = _data.Get<IList<SkillDetails>>(
            "skillList"
        );

        var index = 0;
        foreach (var skillItem in skillList)
        {
            result.Add(
                new GuiLayoutControlDataModel
                {
                    Id = $"onscreen_skill_selection-skill-{index}-button",
                    Sort = index * 2,
                    TemplateId = "platform-button",
                    Options = new GuiControlOptionsModel
                    {
                        {
                            "__metadata",
                            new GuiControlOptionsModel.GuiControlMetadataOptionModel
                            {
                                ModelOptions = new List<string>{ "textBlockOptions" }
                            }
                        },
                        { "width", "160px" },
                        { "height", "20px" },
                        { "fontSize", "12px" },
                        { "color", "white" },
                        { "background", "red" },
                        { "hoverCursor", "pointer" },
                        { "horizontalAlignment", 2 },
                        { "verticalAlignment", 2 },
                        { "onClick", skillItem.OnClick },
                        {
                            "textBlockOptions",
                            new GuiControlOptionsModel
                            {
                                { "text", skillItem.SkillName },
                                { "resizeToFit", true },
                                { "textHorizontalAlignment", 2 },
                            }
                        }
                    }
                }
            );
            // Add spacer element between skill buttons
            if (index + 1 < skillList.Count)
            {
                result.Add(
                    new GuiLayoutControlDataModel
                    {
                        Id = $"onscreen_skill_selection-skill-{index}-spacer",
                        Sort = (index * 2) + 1,
                        TemplateId = "platform-spacer",
                        Options = new GuiControlOptionsModel
                        {
                            { "padding", 5 },
                        }
                    }
                );
            }
            index++;
        }

        return result;
    }
}
