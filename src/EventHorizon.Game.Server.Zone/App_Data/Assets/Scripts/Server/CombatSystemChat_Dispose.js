/**
 * $services: {
 *   logger: ILogger;
 *   eventService: IEventService;
 *   commandService: ICommandService;
 * }
 * $data: {
 *  eventsToDispose: Array<{ name:string; handler: ()=>void; context: any; }>;
 * }
 */

$services.logger.debug("Hello from Server Dispose", $services);

var eventsToRemove = $data.eventsToDispose || [];
eventsToRemove.forEach(eventData => {
    $services.eventService.removeEventListener(
        {
            key: eventData.name
        },
        eventData.handler,
        eventData.context
    );
});
