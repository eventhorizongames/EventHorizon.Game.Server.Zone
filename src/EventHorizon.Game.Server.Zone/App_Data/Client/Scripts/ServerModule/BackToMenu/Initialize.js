/**
 * $services: {
 *   logger: ILogger;
 *   eventService: IEventService;
 *   commandService: ICommandService;
 * }
 * $state: {
 *  onMessageFromCombatSystemHandler: (data) => void;
 * }
 */

$services.logger.debug("Back To Menu - Initialize");

const onBackToMainMenu = () => {
    // Go back to main Menu
    $services.commandService.send({
        type: {
            key: "Engine.Gui.HIDE_LAYOUT_COMMAND",
        },
        data: {
            layoutId: "ChatLayout",
        },
    });
};

$services.commandService.send({
    type: {
        key: "Gui.CREATE_GUI_COMMAND",
    },
    data: {
        layoutList: [
            {
                id: "back_to_main_menu",
                sort: 0,
                controlList: [
                    {
                        id: "back_to_main_menu-container",
                        sort: 0,
                        controlList: [
                            {
                                id: "back_to_main_menu-panel",
                                sort: 0,
                                controlList: [
                                    {
                                        id: "back_to_main_menu-button",
                                        sort: 0,
                                    },
                                ],
                            },
                        ],
                    },
                ],
            },
        ],
        templateList: [
            {
                id: "back_to_main_menu-container",
                type: "Container",
                options: {
                    width: "100%",
                    height: "100%",
                    horizontalAlignment: 0,
                    verticalAlignment: 0,
                    background: "transparent",
                },
            },
            {
                id: "back_to_main_menu-panel",
                type: "Panel",
                options: {
                    verticalAlignment: 0,
                    horizontalAlignment: 0,
                    top: 15,
                    left: 15,
                    enableScrolling: true,
                },
            },
            {
                id: "back_to_main_menu-button",
                type: "Button",
                options: {
                    width: "130px",
                    height: "35px",
                    text: "",
                    textSize: 16,
                    textColor: "white",
                    backgroundColor: "black",
                    disabledColor: "gray",
                    disabledHoverCursor: "mouse",
                    alignment: 2,
                    vAlignment: 2,
                    borderThickness: 0,
                    text: "Back to Main Menu TODO",
                },
            },
        ],
    },
});

// Register New GUI Control's from Templates
$services.logger.debug(
    "containerResult",
    $services.commandService.send({
        type: {
            key: "GUI.REGISTER_CONTROL_COMMAND",
        },
        data: {
            controlId: "back_to_main_menu-container",
            templateId: "back_to_main_menu-container",
        },
    })
);
$services.logger.debug(
    "panelResult",
    $services.commandService.send({
        type: {
            key: "GUI.REGISTER_CONTROL_COMMAND",
        },
        data: {
            controlId: "back_to_main_menu-panel",
            templateId: "back_to_main_menu-panel",
        },
    })
);
$services.logger.debug(
    "inputResult",
    $services.commandService.send({
        type: {
            key: "GUI.REGISTER_CONTROL_COMMAND",
        },
        data: {
            controlId: "back_to_main_menu-button",
            templateId: "back_to_main_menu-button",
            options: {
                // TODO: In the future this will be fixed
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
    })
);

$services.commandService.send({
    type: {
        key: "GUI.ACTIVATE_LAYOUT_COMMAND",
    },
    data: {
        layoutId: "back_to_main_menu",
    },
});
