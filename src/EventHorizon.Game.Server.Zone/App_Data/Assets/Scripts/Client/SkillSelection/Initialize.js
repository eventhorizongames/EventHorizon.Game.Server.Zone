/**
 * This the internal "state" of the script, only accessible by the script.
 * $state: {
 * };
 * 
 * This is data passed to the script from the outside.
 * $data: {
 * };
 */

const skillList = $data.skillList;
const keyboardShortcuts = $state._keyboardShortcuts = [];

// Register New GUI Control's from Templates
$services.commandService.send({
    type: {
        key: "GUI.REGISTER_CONTROL_COMMAND"
    },
    data: {
        controlId: "onscreen_skill_selection-grid",
        templateId: "onscreen_skill_selection-grid"
    }
});
$services.commandService.send({
    type: {
        key: "GUI.REGISTER_CONTROL_COMMAND"
    },
    data: {
        controlId: "onscreen_skill_selection-panel",
        templateId: "onscreen_skill_selection-panel"
    }
});

// Active the Skill Selection GUI
const skillSelectionLayoutId = "onscreen_skill_selection";
$services.commandService.send({
    type: {
        key: "GUI.ACTIVATE_LAYOUT_COMMAND"
    },
    data: {
        layoutId: skillSelectionLayoutId
    }
});

// Create Skill List Panel
const panelControlId = "onscreen_skill_selection-panel";
const panelLayoutControlList = insertSpacer(
    skillList.map((_, index) => ({
        id: `onscreen_skill_selection-skill-${index}-button`,
        sort: index,
        controlList: []
    }))
);

// Add Panel Layout Control List to panelControl 
$services.commandService.send({
    type: {
        key: "GUI.ADD_LAYOUT_TO_CONTROL_COMMAND"
    },
    data: {
        targetControlId: panelControlId,
        registerControlList: [
            ...skillList.map((_, index) => ({
                controlId: `onscreen_skill_selection-skill-${index}-spacer`,
                templateId: `onscreen_skill_selection-skill-spacer`
            })),
            ...skillList.map((skill, index) => ({
                controlId: `onscreen_skill_selection-skill-${index}-button`,
                templateId: `onscreen_skill_selection-skill-button`,
                options: {
                    text: skill.skillName,
                    onClick: skill.onClick,
                    linkOffsetY: index * -50
                }
            }))
        ],
        templateList: [],
        layout: {
            id: "onscreen_skill_selection-panel-skill_list",
            count: 0,
            controlList: panelLayoutControlList
        }
    }
});

// Setup Keyboard Shortcuts
setupKeyboardShortcuts(skillList);


function insertSpacer(skillList /* any[] */ ) {
    var newList = [];
    skillList.forEach((skill, index) => {
        skill.sort = index * 2;
        newList.push(skill);
        if ($utils.isObjectDefined(skillList[index + 1])) {
            newList.push({
                id: `onscreen_skill_selection-skill-${index}-spacer`,
                sort: skill.sort + 1
            });
        }
    });
    return newList;
}

function setupKeyboardShortcuts(skillList) {
    skillList.forEach(skill =>
        skill.keyboardShortcut &&
        keyboardShortcuts.push({
            key: skill.keyboardShortcut,
            pressed: skill.onClick
        })
    );
    keyboardShortcuts.forEach(keyboardShortcut =>
        $services.commandService.send({
            type: {
                key: "INPUT.REGISTER_INPUT_COMMAND"
            },
            data: keyboardShortcut
        })
    );
}