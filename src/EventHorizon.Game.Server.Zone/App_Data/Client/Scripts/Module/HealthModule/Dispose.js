/**
 *
 * $services: {
 *  eventService: {
 *      publish: (event: IEvent)
 *  }
 *  commandService: {
 *      send: (command: ICommand)
 *  }
 * }
 *
 * This the internal "state" of the script, only accessible by the script.
 * $state: {
 * };
 *
 * This is data passed to the script from the outside.
 * $data: {
 *  $entity: IObjectEntity
 *  eventsToDispose: IEventToDispose
 * };
 */

var eventsToRemove = $data.eventsToDispose || [];
eventsToRemove.forEach(eventData => {
    $services.eventService.off(
        {
            key: eventData.name,
        },
        eventData.handler,
        eventData.context
    );
});

$services.commandService.send({
    type: {
        key: "Engine.Gui.DISPOSE_OF_GUI_COMMAND",
    },
    data: {
        id: $data.guiId,
    },
});
