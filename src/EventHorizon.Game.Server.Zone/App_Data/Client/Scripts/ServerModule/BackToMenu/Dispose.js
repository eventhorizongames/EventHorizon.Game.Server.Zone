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

$services.logger.debug("Back To Menu - Dispose", {
    $services,
    $data,
    $state,
});
const guiId = "GUI_BackToMenu.json";

$services.commandService.send(
    $utils.createEvent("Engine.Gui.DISPOSE_OF_GUI_COMMAND", {
        id: guiId,
    })
);
