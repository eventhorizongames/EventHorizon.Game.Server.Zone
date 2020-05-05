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

$services.logger.debug("Player Captures Log - Dispose Script");

var eventsToRemove = $data.eventsToRemove || [];
eventsToRemove.forEach(eventData => {
    $services.eventService.off(
        {
            key: eventData.name
        },
        eventData.handler,
        eventData.context
    );
});
