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

$services.logger.debug("Dispose", $services, {
    $services,
    $data,
    $state,
});
const guiId = "GUI_LeaderBoard.json";

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

// Unregister Input for LeaderBoard Keyboard Shortcut
if ($data.leaderBoardKeyboardShortcut) {
    $services.commandService.send(
        $utils.createEvent(
            "Engine.Input.UNREGISTER_INPUT_COMMAND",
            $data.leaderBoardKeyboardShortcut
        )
    );
}
