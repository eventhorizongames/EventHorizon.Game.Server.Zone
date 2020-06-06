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
            senderControlOptions: {
                color: "yellow",
            },
            messageControlOptions: {},
            message: data.message,
        })
    );
}
