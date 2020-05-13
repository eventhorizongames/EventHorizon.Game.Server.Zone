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
        $utils.createEvent("Engine.Gui.DISPOSE_OF_GUI_COMMAND", {
            id: layoutId,
        })
    );
}
