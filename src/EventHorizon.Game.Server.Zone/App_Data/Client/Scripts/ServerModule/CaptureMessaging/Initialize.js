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
    $services.eventService.publish(
        $utils.createEvent("MessageFromSystem", {
            senderControlOptions: {
                color: "green",
            },
            messageControlOptions: {},
            message: $utils.translation("game:dontHaveTime"),
        })
    );
}
function onTenSecondCaptureMessageHandler(data /* { } */) {
    $services.eventService.publish(
        $utils.createEvent("MessageFromSystem", {
            senderControlOptions: {
                color: "green",
            },
            messageControlOptions: {},
            message: $utils.translation("game:stillHaveTime"),
        })
    );
}
