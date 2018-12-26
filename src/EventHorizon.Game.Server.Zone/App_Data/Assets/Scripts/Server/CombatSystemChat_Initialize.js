/**
 * $services: {
 *   logger: ILogger;
 *   eventService: IEventService;
 *   commandService: ICommandService;
 * }
 */

let messageCount = 0;
$services.logger.debug("Hello from Server Init", $services);
$services.logger.log("Hello from Server Init");
// Setup Event Listeners
$services.eventService.addEventListener({
        key: "MessageFromCombatSystem"
    },
    (data) => {
        messageCount++;
        $services.logger.debug("Message Data", data);
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
                controlTemplateList: [{
                    // Stack Panel, horizontal
                    id: "ChatModule_Panel_" + messageCount,
                    type: "Panel",
                    options: {
                        top: -45,
                        isVertical: false,
                        verticalAlignment: 1,
                        horizontalAlignment: 0,
                        isPointerBlocker: true
                    }
                }, {
                    // Sender Text
                    id: "ChatModule_Sender_" + messageCount,
                    type: "Text",
                    options: {
                        alpha: 1,
                        text: "System: ",
                        background: "red",
                        resizeToFit: true,
                        color: "white",
                        width: "30px",
                        height: "20px",
                        fontSize: "14px",
                        fontWeight: "bold"
                    }
                }, {
                    // Message Text
                    id: "ChatModule_Message_" + messageCount,
                    type: "Text",
                    options: {
                        alpha: 1,
                        text: data.message,
                        color: "white",
                        width: "600px",
                        height: "20px",
                        fontSize: "14px",
                        textHorizontalAlignment: 0
                    }
                }],
                layout: {
                    id: "ChatModule_Panel_" + messageCount,
                    count: 0,
                    controlList: [{
                        id: "ChatModule_Sender_" + messageCount,
                        sort: 0
                    }, {
                        id: "ChatModule_Message_" + messageCount,
                        sort: 1
                    }]
                }
            }

        })
        $services.logger.debug("Message Result", result);
    },
    this);
// Add GUI Control Templates
const containerResult = $services.commandService.send({
    type: {
        key: "GUI.ADD_CONTROL_COMMAND"
    },
    data: {
        controlId: "ChatModule_Container",
        template: {
            id: "ChatModule_Container",
            type: "Container",
            options: {
                width: "33%",
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
        }
    }
});
$services.logger.debug("containerResult", containerResult);
const panelResult = $services.commandService.send({
    type: {
        key: "GUI.ADD_CONTROL_COMMAND"
    },
    data: {
        controlId: "ChatModule_Panel",
        template: {
            id: "ChatModule_Panel",
            type: "Panel",
            options: {
                verticalAlignment: 1,
                horizontalAlignment: 0,
                top: -15,
                left: 15,
                enableScrolling: true
            }
        }
    }
});
$services.logger.debug("panelResult", panelResult);
const inputResult = $services.commandService.send({
    type: {
        key: "GUI.ADD_CONTROL_COMMAND"
    },
    data: {
        controlId: "ChatModule_Input",
        template: {
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
                placeholderText: "Type message here...",
                focusedBackground: "black",
                thickness: 0,
                cornerRadius: 0,

                onClick: (text) => $services.logger.debug(text),
            }
        }
    }
});
$services.logger.debug("inputResult", inputResult);
// Add GUI Layout
const layoutResult = $services.commandService.send({
    type: {
        key: "GUI.ADD_LAYOUT_COMMAND"
    },
    data: {
        layout: {
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
        }
    }
});
$services.logger.debug("layoutResult", layoutResult);