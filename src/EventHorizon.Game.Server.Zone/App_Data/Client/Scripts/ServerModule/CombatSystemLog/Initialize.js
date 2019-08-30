/**
 * $services: {
 *   logger: ILogger;
 *   eventService: IEventService;
 *   commandService: ICommandService;
 * }
 * $state: {
 *  onMessageFromCombatSystemHandler: (data) => void;
 * }
 */

const layoutId = "GUI_CombatSystemLog.json";
const guiId = layoutId;
let messageCount = 0;
$services.logger.debug("Combat System Log - Initialize");

// Create and Activate GUI
$services.commandService.send(
    $utils.createEvent("Engine.Gui.CREATE_GUI_COMMAND", {
        id: guiId,
        layoutId,
    })
);
$services.commandService.send(
    $utils.createEvent("Engine.Gui.ACTIVATE_GUI_COMMAND", {
        id: guiId,
    })
);

// Setup Event Listeners
$services.eventService.on(
    {
        key: "MessageFromCombatSystem",
    },
    onMessageFromCombatSystemHandler,
    this
);
$services.eventService.on(
    {
        key: "MessageFromCombatSystem.SHOW",
    },
    onShowMessageFromCombatSystemHandler,
    this
);
$services.eventService.on(
    {
        key: "MessageFromCombatSystem.HIDE",
    },
    onHideMessageFromCombatSystemHandler,
    this
);

// Add Events to the $data, that can be disposed of later
$data.eventsToRemove = [];
$data.eventsToRemove.push({
    name: "MessageFromCombatSystem",
    handler: onMessageFromCombatSystemHandler,
    context: this,
});
$data.eventsToRemove.push({
    name: "MessageFromCombatSystem.SHOW",
    handler: onShowMessageFromCombatSystemHandler,
    context: this,
});
$data.eventsToRemove.push({
    name: "MessageFromCombatSystem.HIDE",
    handler: onHideMessageFromCombatSystemHandler,
    context: this,
});

// Private functions
function onMessageFromCombatSystemHandler(data) {
    messageCount++;
    $services.logger.debug("Message Data", data);
    if (!data.message) {
        $services.logger.debug("No message", data);
        return;
    }
    const newMessageLayoutId = "GUI_CombatSystemLog-Message.json";
    const newMessageGuiId = `GUI_CombatSystemLog-Message_${messageCount}`;
    $services.commandService.send(
        $utils.createEvent("Engine.Gui.CREATE_GUI_COMMAND", {
            id: newMessageGuiId,
            layoutId: newMessageLayoutId,
            parentControlId: $services.queryService.query({
                type: "Engine.Gui.QUERY_FOR_GENERATE_GUI_CONTROL_ID",
                data: {
                    guiId,
                    controlId: "gui_combat_system_log-panel",
                },
            }).result,
            controlDataList: [
                {
                    controlId: "gui_combat_system_message-message",
                    options: {
                        text: data.message,
                    },
                },
            ],
        })
    );
    $services.commandService.send(
        $utils.createEvent("Engine.Gui.ACTIVATE_GUI_COMMAND", {
            id: newMessageGuiId,
        })
    );
}

function onShowMessageFromCombatSystemHandler() {
    // Show the System Log Window
    $services.commandService.send(
        $utils.createEvent("Engine.Gui.SHOW_GUI_COMMAND", {
            id: guiId,
        })
    );
}
function onHideMessageFromCombatSystemHandler() {
    // Hide the System Log Window
    $services.commandService.send(
        $utils.createEvent("Engine.Gui.HIDE_GUI_COMMAND", {
            id: guiId,
        })
    );
}
