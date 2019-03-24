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

let messageCount = 0;
let showingChat = false;
$services.logger.debug("Combat System Chat - Initialize");
$services.logger.log("Combat System Chat - Initialize");

const onMessageFromCombatSystemHandler = (data) => {
    messageCount++;
    $services.logger.debug("Message Data", data);
    if (!showingChat) {
        $services.logger.debug("Chat not open");
        return;
    }
    if (!data.message) {
        $services.logger.debug("No message", data);
        return;
    }
    var result = $services.commandService.send({
        type: {
            key: "GUI.ADD_LAYOUT_TO_CONTROL_COMMAND"
        },
        data: {
            targetControlId: "ChatModule_Panel",
            registerControlList: [{
                controlId: "ChatModule_Message_Panel_" + messageCount,
                templateId: "ChatModule_Message_Panel",
            }, {
                controlId: "ChatModule_Sender_" + messageCount,
                templateId: "ChatModule_Sender",
                options: {
                    text: "System : "
                }
            }, {
                controlId: "ChatModule_Message_" + messageCount,
                templateId: "ChatModule_Message",
                options: {
                    text: data.message
                }
            }],
            templateList: [],
            layout: {
                id: "ChatModule_Panel_" + messageCount,
                count: 0,
                controlList: [{
                    id: "ChatModule_Message_Panel_" + messageCount,
                    sort: 0,
                    controlList: [{
                        id: "ChatModule_Sender_" + messageCount,
                        sort: 0
                    }, {
                        id: "ChatModule_Message_" + messageCount,
                        sort: 1
                    }]
                }]
            }
        }
    })
    $services.logger.debug("Message Result", result);
};


const onShowMessageFromCombatSystemHandler = () => {
    // Active the Chat Window
    $services.commandService.send({
        type: {
            key: "GUI.ACTIVATE_LAYOUT_COMMAND"
        },
        data: {
            layoutId: "ChatLayout"
        }
    });
    showingChat = true;
};
const onHideMessageFromCombatSystemHandler = () => {
    // Active the Chat Window
    $services.commandService.send({
        type: {
            key: "GUI.IN_ACTIVATE_LAYOUT_COMMAND"
        },
        data: {
            layoutId: "ChatLayout"
        }
    });
    // Active the Chat Window
    $services.commandService.send({
        type: {
            key: "GUI.IN_ACTIVATE_LAYOUT_COMMAND"
        },
        data: {
            layoutId: "ChatLayout"
        }
    });
    showingChat = false;
};



// Setup Event Listeners
$services.eventService.addEventListener({
        key: "MessageFromCombatSystem"
    },
    onMessageFromCombatSystemHandler,
    this
);
$services.eventService.addEventListener({
        key: "MessageFromCombatSystem.SHOW"
    },
    onShowMessageFromCombatSystemHandler,
    this
);
$services.eventService.addEventListener({
        key: "MessageFromCombatSystem.HIDE"
    },
    onHideMessageFromCombatSystemHandler,
    this
);

// Add onMessageFromCombatSystem to the $data
$data.eventsToRemove = [];
$data.eventsToRemove.push({
    name: "MessageFromCombatSystem",
    handler: onMessageFromCombatSystemHandler,
    context: this
});
$data.eventsToRemove.push({
    name: "MessageFromCombatSystem.SHOW",
    handler: onShowMessageFromCombatSystemHandler,
    context: this
});
$data.eventsToRemove.push({
    name: "MessageFromCombatSystem.HIDE",
    handler: onHideMessageFromCombatSystemHandler,
    context: this
});

$services.eventService.publish({
    eventType: {
        key: "GUI.CREATE_GUI_EVENT"
    },
    data: {
        layoutList: [{
            id: "ChatLayout",
            sort: 0,
            controlList: [{
                id: "ChatModule_Container",
                sort: 0,
                controlList: [{
                    id: "ChatModule_Panel",
                    sort: 0
                    // },{
                    //     id: "ChatModule_Input",
                    //     sort: 1
                }]
            }]
        }],
        templateList: [{
            id: "ChatModule_Container",
            type: "Container",
            options: {
                width: "66%",
                height: "50%",
                alpha: 0.5,
                horizontalAlignment: 0,
                verticalAlignment: 1,
                background: "black",
                cornerRadius: 20,
                left: 20,
                top: -20,
                thickness: 0
            }
        }, {
            id: "ChatModule_Panel",
            type: "Panel",
            options: {
                verticalAlignment: 1,
                horizontalAlignment: 0,
                top: -15,
                left: 15,
                enableScrolling: true
            }
        }, {
            id: "ChatModule_Input",
            type: "Input",
            options: {
                alpha: 2,
                autoStretchWidth: false,
                width: "100%",
                height: "15%",
                fontSize: 15,
                horizontalAlignment: 0,
                verticalAlignment: 1,
                color: "white",
                background: "black",
                focusedBackground: "black",
                thickness: 0,
                cornerRadius: 0,

                placeholderText: "message_placeholder",
                onClick: (_) => {},
            }
        }, {
            // Stack Panel, horizontal
            id: "ChatModule_Message_Panel",
            type: "Panel",
            options: {
                top: -45,
                isVertical: false,
                verticalAlignment: 1,
                horizontalAlignment: 0,
                isPointerBlocker: false
            }
        }, {
            // Sender Text
            id: "ChatModule_Sender",
            type: "Text",
            options: {
                alpha: 1,
                background: "red",
                resizeToFit: true,
                color: "white",
                width: "30px",
                height: "20px",
                fontSize: "14px",
                fontWeight: "bold",

                text: "sender_text",
            }
        }, {
            // Message Text
            id: "ChatModule_Message",
            type: "Text",
            options: {
                alpha: 1,
                color: "white",
                width: "600px",
                height: "20px",
                fontSize: "14px",
                textHorizontalAlignment: 0,

                text: "message_text",
            }
        }]
    }
});

// Register New GUI Control's from Templates
$services.logger.debug("containerResult", $services.commandService.send({
    type: {
        key: "GUI.REGISTER_CONTROL_COMMAND"
    },
    data: {
        controlId: "ChatModule_Container",
        templateId: "ChatModule_Container"
    }
}));
$services.logger.debug("panelResult", $services.commandService.send({
    type: {
        key: "GUI.REGISTER_CONTROL_COMMAND"
    },
    data: {
        controlId: "ChatModule_Panel",
        templateId: "ChatModule_Panel"
    }
}));
$services.logger.debug("inputResult", $services.commandService.send({
    type: {
        key: "GUI.REGISTER_CONTROL_COMMAND"
    },
    data: {
        controlId: "ChatModule_Input",
        templateId: "ChatModule_Input",
        options: {
            placeholderText: "Type message here...",
            onClick: (text) => $services.logger.debug(text),
        }
    }
}));