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
        key: "GUI.DISPOSE_OF_GUI_CONTROL_COMMAND",
    },
    data: {
        controlId: $data.guiControlId,
    },
});
