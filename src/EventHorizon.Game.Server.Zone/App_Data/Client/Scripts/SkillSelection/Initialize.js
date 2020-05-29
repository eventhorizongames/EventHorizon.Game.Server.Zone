/**
 * This the internal "state" of the script, only accessible by the script.
 * $state: {
 * };
 *
 * This is data passed to the script from the outside.
 * $data: {
 * };
 */

const layoutId = (guiId = "GUI_Module_SkillSelection.json");
const skillList = $data.skillList;
const keyboardShortcuts = ($data.keyboardShortcuts = []);

/**
 * When the Skill Selection GUI is Activated it will publish an event and call this function.
 * This function will populate the GUI with the "active" skill list of the entity.
 */
function guiActivated() {
    const skillListLayoutId = (skillListGuiId =
        "GUI_Module_SkillSelection.SkillList");
    const skillListLayoutData = {
        id: skillListLayoutId,
        sort: 0,
        controlList: skillLayoutControlList,
    };
    $services.commandService.send(
        $utils.createEvent("Engine.Gui.REGISTER_GUI_LAYOUT_DATA_COMMAND", {
            layoutData: skillListLayoutData,
        })
    );

    // Create new Layout attached to "onscreen_skill_selection-panel"
    $services.commandService.send(
        $utils.createEvent("Engine.Gui.CREATE_GUI_COMMAND", {
            id: skillListGuiId,
            layoutId: skillListLayoutId,
            parentControlId: $services.queryService.query({
                type: "Engine.Gui.QUERY_FOR_GENERATE_GUI_CONTROL_ID",
                data: {
                    guiId,
                    controlId: "onscreen_skill_selection-panel",
                },
            }).result,
        })
    );
    $services.commandService.send(
        $utils.createEvent("Engine.Gui.ACTIVATE_GUI_COMMAND", {
            id: skillListGuiId,
        })
    );
}

// Create the dynamic skill control list
const skillLayoutControlList = insertSpacer(
    skillList.map((skill, index) => ({
        id: `onscreen_skill_selection-skill-${index}-button`,
        sort: index,
        templateId: "platform-button",
        options: {
            width: "120px",
            height: "20px",
            fontSize: 12,
            color: "white",
            background: "red",
            alignment: 2,
            vAlignment: 0,
            borderThickness: 0,
            text: skill.skillName,
            onClick: skill.onClick,
            linkOffsetY: index * -50,
        },
    }))
);
function insertSpacer(skillButtonControlList /* any[] */) {
    var newList = [];
    skillButtonControlList.forEach((skillControl, index) => {
        skillControl.sort = index * 2;
        newList.push(skillControl);
        if ($utils.isObjectDefined(skillButtonControlList[index + 1])) {
            newList.push({
                id: `onscreen_skill_selection-skill-${index}-spacer`,
                sort: skillControl.sort + 1,
                templateId: "platform-spacer",
                options: {
                    padding: 5,
                },
            });
        }
    });
    return newList;
}

$services.eventService.on(
    { key: "Local.SkillSelection.Gui.ACTIVATED" },
    guiActivated,
    "SkillSelection-Initialize"
);
$services.commandService.send(
    $utils.createEvent("Engine.Gui.CREATE_GUI_COMMAND", {
        id: guiId,
        layoutId,
        controlDataList: [],
    })
);
$services.commandService.send(
    $utils.createEvent("Engine.Gui.ACTIVATE_GUI_COMMAND", {
        id: guiId,
    })
);

// Create a List of Events to Remove
$data.eventsToRemove = [];
$data.eventsToRemove.push({
    name: "Local.SkillSelection.Gui.ACTIVATED",
    handler: guiActivated,
    context: "SkillSelection-Initialize",
});

// Setup Keyboard Shortcuts
setupKeyboardShortcuts(skillList);

function setupKeyboardShortcuts(skillList) {
    skillList.forEach(
        (skill) =>
            skill.keyboardShortcut &&
            keyboardShortcuts.push({
                key: skill.keyboardShortcut,
                pressed: skill.onClick,
            })
    );
    keyboardShortcuts.forEach((keyboardShortcut) =>
        $services.commandService.send(
            $utils.createEvent(
                "Engine.Input.REGISTER_INPUT_COMMAND",
                keyboardShortcut
            )
        )
    );
}
