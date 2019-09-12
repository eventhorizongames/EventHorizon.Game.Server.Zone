/**
 * $services: {
 *   logger: ILogger;
 *   commandService: ICommandService;
 * }
 * $state: {
 * }
 */

const layoutId = "gui_dialog";
$services.commandService.send(
    $utils.createEvent("Engine.Gui.CREATE_GUI_COMMAND", {
        id: layoutId,
        layoutId,
    })
);
$services.commandService.send(
    $utils.createEvent("Engine.Gui.ACTIVATE_GUI_COMMAND", {
        id: layoutId,
    })
);