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

$services.logger.debug("Dispose", $services);
const guiId = "GUI_BackToMenu.json";

$services.commandService.send(
    $utils.createEvent("Engine.Gui.DISPOSE_OF_GUI_COMMAND", {
        id: guiId,
    })
);
