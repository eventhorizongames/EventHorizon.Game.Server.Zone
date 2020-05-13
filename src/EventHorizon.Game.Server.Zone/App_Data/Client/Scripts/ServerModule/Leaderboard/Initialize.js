/**
 * $services: {
 *   logger: ILogger;
 *   commandService: ICommandService;
 * }
 * $state: {
 * }
 */

const layoutId = "GUI_LeaderBoard.json";
const guiId = layoutId;
$services.logger.debug("Leader Board - Initialize");

$services.commandService.send(
    $utils.createEvent("Engine.Gui.CREATE_GUI_COMMAND", {
        id: guiId,
        layoutId,
        controlDataList: [],
    })
);
$services.commandService.send(
    $utils.createEvent("Engine.Gui.ACTIVATE_GUI_COMMAND", {
        id: guiId,
    })
);
