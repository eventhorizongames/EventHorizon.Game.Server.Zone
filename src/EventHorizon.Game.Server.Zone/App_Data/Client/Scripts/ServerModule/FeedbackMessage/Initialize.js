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

$services.logger.debug("Feedback Message - Initialize Script", {
    $services,
    $data,
    $state,
});
// Setup GUI
var layoutId = "GUI_FeedbackMessage.json";
$services.commandService.send(
    $utils.createEvent("Engine.Gui.CREATE_GUI_COMMAND", {
        id: layoutId,
        layoutId,
    })
);
$services.commandService.send(
    $utils.createEvent("Engine.Gui.ACTIVATE_GUI_COMMAND", {
        id: layoutId,
    })
);

// Setup Event Listeners
$services.eventService.on(
    {
        key: "DisplayFeedbackMessage",
    },
    onMessageHandler,
    this
);

// Add Events to the $data, that can be disposed of later
$data.eventsToRemove = [];
$data.eventsToRemove.push({
    name: "DisplayFeedbackMessage",
    handler: onMessageHandler,
    context: this,
});

// Private functions
function onMessageHandler(data /* { message: string; } */) {
    $services.eventService.publish(
        $utils.createEvent("MessageFromSystem", {
            senderControlOptions: {},
            messageControlOptions: {
                color: "red",
            },
            message: data.message,
        })
    );
    $services.commandService.send(
        $utils.createEvent("Engine.Gui.UPDATE_GUI_CONTROL_COMMAND", {
            guiId: layoutId,
            control: {
                controlId: "feedback_message-text",
                options: {
                    color: "red",
                    text: data.message,
                },
            },
        })
    );
    $services.commandService.send(
        $utils.createEvent("Engine.Gui.SHOW_GUI_COMMAND", {
            id: layoutId,
        })
    );
    setTimeout(() => {
        $services.commandService.send(
            $utils.createEvent("Engine.Gui.HIDE_GUI_COMMAND", {
                id: layoutId,
            })
        );
    }, 5000);
}
