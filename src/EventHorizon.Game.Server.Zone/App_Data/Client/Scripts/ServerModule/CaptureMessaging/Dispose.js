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

$services.logger.debug("Capture Messaging - Dispose Script", {
    $services,
    $data,
    $state,
});
const guiId = "GUI_CaptureMessaging.json";

$services.commandService.send(
    $utils.createEvent("Engine.Gui.DISPOSE_OF_GUI_COMMAND", {
        id: guiId,
    })
);

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
