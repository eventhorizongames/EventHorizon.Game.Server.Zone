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

$services.logger.debug("Capture Messaging - Initialize Script", {
    $services,
    $data,
    $state,
});
var layoutId = "GUI_CaptureMessaging.json";
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
        key: "Server.SHOW_FIVE_SECOND_CAPTURE_MESSAGE",
    },
    onFiveSecondCaptureMessageHandler,
    this
);
$services.eventService.on(
    {
        key: "Server.SHOW_TEN_SECOND_CAPTURE_MESSAGE",
    },
    onTenSecondCaptureMessageHandler,
    this
);

// Add Events to the $data, that can be disposed of later
$data.eventsToRemove = [];
$data.eventsToRemove.push({
    name: "Server.SHOW_FIVE_SECOND_CAPTURE_MESSAGE",
    handler: onFiveSecondCaptureMessageHandler,
    context: this,
});
$data.eventsToRemove.push({
    name: "Server.SHOW_TEN_SECOND_CAPTURE_MESSAGE",
    handler: onTenSecondCaptureMessageHandler,
    context: this,
});

// Private functions
function onFiveSecondCaptureMessageHandler(data /* { } */) {
    showMessage($utils.translation("game:dontHaveTime"));
}
function onTenSecondCaptureMessageHandler(data /* { } */) {
    showMessage($utils.translation("game:stillHaveTime"));
}

function showMessage(message) {
    $services.eventService.publish(
        $utils.createEvent("MessageFromSystem", {
            senderControlOptions: {
                color: "green",
            },
            messageControlOptions: {},
            message,
        })
    );
    $services.commandService.send(
        $utils.createEvent("Engine.Gui.UPDATE_GUI_CONTROL_COMMAND", {
            guiId: layoutId,
            control: {
                controlId: "capture_messaging-text",
                options: {
                    text: message,
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
    }, 3000);
}
