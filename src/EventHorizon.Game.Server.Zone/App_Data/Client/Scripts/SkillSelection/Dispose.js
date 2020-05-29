/**
 * This the internal "state" of the script, only accessible by the script.
 * $state: {
 * };
 *
 * This is data passed to the script from the outside.
 * $data: {
 * };
 */

var eventsToRemove = $data.eventsToDispose || [];
eventsToRemove.forEach((eventData) => {
    $services.eventService.off(
        {
            key: eventData.name,
        },
        eventData.handler,
        eventData.context
    );
});

var keyboardRegistrationsToRemove = $data.keyboardShortcuts || [];
keyboardRegistrationsToRemove.forEach((keyboardShortcut) => {
    $services.commandService.send(
        $utils.createEvent(
            "Engine.Input.UNREGISTER_INPUT_COMMAND",
            keyboardShortcut
        )
    );
});
