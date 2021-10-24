/*
data:
    active: bool
    observer: ObserverBase
    skillList: IList<SkillDetails>
*/

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

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
using EventHorizon.Game.Client.Engine.Systems.Scripting.Run;
using EventHorizon.Game.Server.ServerModule.SystemLog.Hide;
using EventHorizon.Game.Server.ServerModule.SystemLog.Message;
using EventHorizon.Game.Server.ServerModule.SystemLog.Show;
using EventHorizon.Game.Server.SkillSelection.Activated;
using EventHorizon.Game.Server.SkillSelection.Model;
using EventHorizon.Game.Client.Engine.Systems.ClientAction.Api;
using EventHorizon.Game.Client.Core.I18n.Api;

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
        var entity = data.Get<IObjectEntity>("entity");

        var skillListId = "skillList";
        var layoutId = "GUI_Module_SkillSelection.json";
        var guiId = layoutId;

        data.Set(
            skillListId,
            CreatePlayerSkillList(
                logger,
                services,
                entity
            )
        );

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


    private IEnumerable<SkillDetails> CreatePlayerSkillList(
        ILogger logger,
        ScriptServices services,
        IObjectEntity entity
    )
    {
        return new List<SkillDetails>
        {
            new SkillDetails
            {
                SkillName = "Clear Selection",
                OnClick = async () =>
                {
                    // TODO: [Interaction System] - Allow for Selection using SelectionModule
                    logger.LogDebug("RUN Clear Selection");
                    await services.Mediator.Send(
                        new RunClientScriptCommand(
                            "Skill_Player_ClearSelection",
                            "skill.client_selection",
                            new Dictionary<string, object>
                            {
                                { "entity", entity },
                            }
                        )
                    );
                }
            },
            new SkillDetails
            {
                SkillName = "Open Debugging",
                OnClick = () =>
                {
                    // TODO: [DEBUGGING] - RUN Open Debugging
                    logger.LogDebug("TODO: RUN Open Debugging");
                    return Task.CompletedTask;
                }
            },
            new SkillDetails
            {
                SkillName = "Close Debugging",
                OnClick = () =>
                {
                    // TODO: [DEBUGGING] - RUN Close Debugging
                    logger.LogDebug("RUN Close Debugging");
                    return Task.CompletedTask;
                }
            },
            new SkillDetails
            {
                SkillName = "Test Debugging Message",
                OnClick = () =>
                {
                    // TODO: [DEBUGGING] - RUN Test Debugging Message
                    logger.LogDebug("RUN Test Debugging Message");
                    return Task.CompletedTask;
                }
            },
            new SkillDetails
            {
                SkillName = "Open Dialog",
                OnClick = async () =>
                {
                    logger.LogDebug("RUN Open Dialog");
                    await services.Mediator.Send(
                        new ShowGuiCommand(
                            "gui_dialog"
                        )
                    );
                }
            },
            new SkillDetails
            {
                SkillName = "Show System Log",
                OnClick = async () =>
                {
                    logger.LogDebug("RUN Show System Log");
                    await services.Mediator.Send(
                        new RunClientScriptCommand(
                            "Log_ShowSystemLog",
                            "skill.show_system_log",
                            new Dictionary<string, object>
                            {
                                { "entity", entity },
                            }
                        )
                    );
                }
            },
            new SkillDetails
            {
                SkillName = "Hide System Log",
                OnClick = async () =>
                {
                    logger.LogDebug("RUN Hide System Log");
                    await services.Mediator.Send(
                        new RunClientScriptCommand(
                            "Log_HideSystemLog",
                            "skill.hide_system_log",
                            new Dictionary<string, object>
                            {
                                { "entity", entity },
                            }
                        )
                    );
                }
            },
            new SkillDetails
            {
                SkillName = "Test System Message",
                OnClick = async () =>
                {
                    logger.LogDebug("RUN Test System Message");
                    // TODO: ClientActionDataResolver needs to be moved into public surface area in Client SDK
                    //await services.Mediator.Publish(
                    //    new ClientActionMessageFromSystemEvent(
                    //        new ClientActionDataResolver(
                    //            services,
                    //            new Dictionary<string, object>
                    //            {
                    //                { "message", "Hello from Skill Selection" }
                    //            }
                    //        )
                    //    )
                    //);
                }
            },
            new SkillDetails
            {
                SkillName = "Fire Ball",
                // KeyboardShortcut = "k",
                OnClick = async () =>
                {
                    logger.LogDebug("RUN Fire Ball");
                    await services.Mediator.Send(
                        new RunClientScriptCommand(
                            "Skill_Player_FireBall",
                            "skill.fireball",
                            new Dictionary<string, object>
                            {
                                { "entity", entity },
                            }
                        )
                    );
                }
            },
            new SkillDetails
            {
                SkillName = "Capture Target",
                // KeyboardShortcut = "c",
                OnClick = async () =>
                {
                    logger.LogDebug("RUN Capture Target");
                    await services.Mediator.Send(
                        new RunClientScriptCommand(
                            "Skill_Player_CaptureTarget",
                            "skill.capture_target",
                            new Dictionary<string, object>
                            {
                                { "entity", entity },
                            }
                        )
                    );
                }
            },
            new SkillDetails
            {
                SkillName = "Companion Fire Ball",
                // KeyboardShortcut = "l",
                OnClick = async () =>
                {
                    logger.LogDebug("RUN Companion Fire Ball");
                    await services.Mediator.Send(
                        new RunClientScriptCommand(
                            "Skill_Runners_RunSelectedCompanionTargetedSkill",
                            "skill.companion_targeted_skill",
                            new Dictionary<string, object>
                            {
                                { "entity", entity },
                                { "skillId", "Skills_FireBall.json" },
                                { "noSelectionsMessage", services.GetService<ILocalizer>()["noSelectionsMessage"] },
                            }
                        )
                    );
                }
            },
        };
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
