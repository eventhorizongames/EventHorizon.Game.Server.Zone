/**
 * $services: {
 *   logger: ILogger;
 *   commandService: ICommandService;
 * }
 * $state: {
 * }
 */

const dialogLayoutIds = ["gui_dialog"];

for (var layoutId of dialogLayoutIds) {
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
}
