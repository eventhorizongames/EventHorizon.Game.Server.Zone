/**
 * $services: {
 *   logger: ILogger;
 *   eventService: IEventService;
 *   commandService: ICommandService;
 * }
 * $data: {
 *  eventsToRemove: Array<{ name, handler, context }>
 * }
 */

const layoutId = "GUI_System_Log.json";
const guiId = layoutId;
let messageCount = 0;
$services.logger.debug("System Log - Initialize Script");

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
        key: "MessageFromSystem",
    },
    onMessageFromSystemHandler,
    this
);
$services.eventService.on(
    {
        key: "MessageFromSystem.SHOW",
    },
    onShowMessageFromSystemHandler,
    this
);
$services.eventService.on(
    {
        key: "MessageFromSystem.HIDE",
    },
    onHideMessageFromSystemHandler,
    this
);

// Add Events to the $data, that can be disposed of later
$data.eventsToRemove = [];
$data.eventsToRemove.push({
    name: "MessageFromSystem",
    handler: onMessageFromSystemHandler,
    context: this,
});
$data.eventsToRemove.push({
    name: "MessageFromSystem.SHOW",
    handler: onShowMessageFromSystemHandler,
    context: this,
});
$data.eventsToRemove.push({
    name: "MessageFromSystem.HIDE",
    handler: onHideMessageFromSystemHandler,
    context: this,
});

// Private functions
function onMessageFromSystemHandler(data /* { senderControlOptions?:any; messageControlOptions?:any; message:string; } */) {
    messageCount++;
    $services.logger.debug("Message Data", data);
    if (!data.message) {
        $services.logger.debug("No message", data);
        return;
    }
    const newMessageLayoutId = "GUI_System_Log-Message.json";
    const newMessageGuiId = `GUI_System_Log-Message_${messageCount}`;
    $services.commandService.send(
        $utils.createEvent("Engine.Gui.CREATE_GUI_COMMAND", {
            id: newMessageGuiId,
            layoutId: newMessageLayoutId,
            parentControlId: $services.queryService.query({
                type: "Engine.Gui.QUERY_FOR_GENERATE_GUI_CONTROL_ID",
                data: {
                    guiId,
                    controlId: "gui_system_log-panel",
                },
            }).result,
            controlDataList: [
                {
                    controlId: "gui_system_message-sender",
                    options: {
                        ...data.senderControlOptions,
                    },
                },
                {
                    controlId: "gui_system_message-message",
                    options: {
                        ...data.messageControlOptions,
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

function onShowMessageFromSystemHandler() {
    // Show the System Log Window
    $services.commandService.send(
        $utils.createEvent("Engine.Gui.SHOW_GUI_COMMAND", {
            id: guiId,
        })
    );
}
function onHideMessageFromSystemHandler() {
    // Hide the System Log Window
    $services.commandService.send(
        $utils.createEvent("Engine.Gui.HIDE_GUI_COMMAND", {
            id: guiId,
        })
    );
}
