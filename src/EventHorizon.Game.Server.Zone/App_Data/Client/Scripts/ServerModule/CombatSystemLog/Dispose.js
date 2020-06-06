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

$services.logger.debug("Combat System Log - Dispose Script", {
    $services,
    $data,
    $state,
});

var eventsToRemove = $data.eventsToRemove || [];
eventsToRemove.forEach((eventData) => {
    $services.eventService.off(
        {
            key: eventData.name,
        },
        eventData.handler,
        eventData.context
    );
});
