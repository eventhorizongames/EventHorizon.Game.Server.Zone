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

$services.logger.debug("Combat System Log - Initialize Script");

// Setup Event Listeners
$services.eventService.on(
    {
        key: "MessageFromCombatSystem",
    },
    onMessageFromCombatSystemHandler,
    this
);

// Add Events to the $data, that can be disposed of later
$data.eventsToRemove = [];
$data.eventsToRemove.push({
    name: "MessageFromCombatSystem",
    handler: onMessageFromCombatSystemHandler,
    context: this,
});

// Private functions
function onMessageFromCombatSystemHandler(data /* { message:string; messageCode?:string; } */) {
    $services.logger.debug("Message Data", data);
    if (!data.message) {
        $services.logger.debug("No message", data);
        return;
    }
    $services.eventService.publish(
        $utils.createEvent("MessageFromSystem", {
            senderControlOptions: {
                color: "red",
                text: $utils.translation("combatSystem:system"),
            },
            messageControlOptions: {

            },
            message: data.message
        })
    );
}
