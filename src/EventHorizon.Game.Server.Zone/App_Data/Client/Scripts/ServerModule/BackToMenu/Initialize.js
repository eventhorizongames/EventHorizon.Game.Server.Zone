/**
 * $services: {
 *   logger: ILogger;
 *   commandService: ICommandService;
 * }
 * $state: {
 * }
 */

const layoutId = "GUI_BackToMenu.json";
const guiId = layoutId;
$services.logger.debug("Back To Menu - Initialize", {
    $services,
    $data,
    $state,
});

$services.commandService.send(
    $utils.createEvent("Engine.Gui.CREATE_GUI_COMMAND", {
        id: guiId,
        layoutId,
        controlDataList: [
            {
                controlId: "back_to_main_menu-button",
                options: {
                    textKey: "account_BackToMainMenu",
                    _text: "Hello",
                    onClick: () => window.location.reload(),
                    // onClick: () =>
                    //     $services.commandService.send({
                    //         type: {
                    //             key: "ClientScenes.START_SCENE_COMMAND",
                    //         },
                    //         data: {
                    //             sceneId: "main-menu",
                    //         },
                    //     }),
                },
            },
        ],
    })
);
$services.commandService.send(
    $utils.createEvent("Engine.Gui.ACTIVATE_GUI_COMMAND", {
        id: guiId,
    })
);
