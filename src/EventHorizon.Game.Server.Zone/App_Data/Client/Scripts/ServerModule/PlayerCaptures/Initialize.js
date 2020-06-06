/**
 * $services: {
 *   logger: ILogger;
 *   eventService: IEventService;
 *   commandService: ICommandService;
 *   queryService: IQueryService;
 * }
 * $data: {
 *  eventsToRemove: Array<{ name, handler, context }>
 * }
 */

const newLayoutId = "GUI_PlayerCaptures-Message.json";
const layoutId = "GUI_PlayerCaptures.json";
const guiId = layoutId;
let caughtCount = 0;
$services.logger.debug("PlayerCaptures Log - Initialize Script", {
    $services,
    $data,
    $state,
});

// Create and Activate GUI
$services.commandService.send(
    $utils.createEvent("Engine.Gui.CREATE_GUI_COMMAND", {
        id: guiId,
        layoutId,
    })
);
$services.commandService.send(
    $utils.createEvent("Engine.Gui.ACTIVATE_GUI_COMMAND", {
        id: guiId,
    })
);

// Setup Event Listeners
$services.eventService.on(
    {
        key: "EntityClientChanged",
    },
    onEntityClientChangedHandler,
    this
);

// Add Events to the $data, that can be disposed of later
$data.eventsToRemove = [];
$data.eventsToRemove.push({
    name: "MessageFromSystem",
    handler: onEntityClientChangedHandler,
    context: this,
});

const gamePlayerCaptureStatePropertyName = "gamePlayerCaptureState";
const createGlobalIdTag = (globalId) => `globalId:${globalId}`;
let currentDisplayedCaught = [];
// Private functions
function onEntityClientChangedHandler(
    { details } /* { details: IObjectEntityDetails } */
) {
    // Get Current Clients Player
    const playerResult = $services.queryService.query(
        $utils.createQuery("Client.Player.GET_CLIENT_PLAYER", {})
    );
    const player = playerResult.result;
    // Validate Updated Entity is this clients Player
    if (!playerResult.success || details.id !== player.entityId) {
        return;
    }

    const companionsCaught =
        details.data[gamePlayerCaptureStatePropertyName].companionsCaught;
    if (!companionsCaught) {
        return;
    }
    if (
        companionsCaught.length === 0 ||
        companionsCaught.length < caughtCount
    ) {
        resetGUI();
        return;
    }
    const companionsCaughtNameList = companionsCaught
        .filter((globalId) => !currentDisplayedCaught.includes(globalId))
        .map(
            (globalId) =>
                $services.queryService.query(
                    $utils.createQuery("Entity.Tracked.QUERY_FOR_ENTITY", {
                        tag: createGlobalIdTag(globalId),
                    })
                ).result[0].name
        );

    currentDisplayedCaught = companionsCaught;
    companionsCaughtNameList.forEach((name) => {
        caughtCount++;
        const newGuiId = `GUI_PlayerCaptures-Message_${caughtCount}`;
        $services.commandService.send(
            $utils.createEvent("Engine.Gui.CREATE_GUI_COMMAND", {
                id: newGuiId,
                layoutId: newLayoutId,
                parentControlId: $services.queryService.query({
                    type: "Engine.Gui.QUERY_FOR_GENERATE_GUI_CONTROL_ID",
                    data: {
                        guiId,
                        controlId: "player_capture-panel",
                    },
                }).result,
                controlDataList: [
                    {
                        controlId: "player_capture-message-immortal-name",
                        options: {
                            text: `Captured Immortal ${name}`,
                        },
                    },
                ],
            })
        );
        $services.commandService.send(
            $utils.createEvent("Engine.Gui.ACTIVATE_GUI_COMMAND", {
                id: newGuiId,
            })
        );
    });
}

function resetGUI() {
    // Rest the UX
    $services.commandService.send(
        $utils.createEvent("Engine.Gui.DISPOSE_OF_GUI_COMMAND", {
            id: guiId,
        })
    );
    // Clear out the current Displayed Caught list
    currentDisplayedCaught = [];
    // Create
    $services.commandService.send(
        $utils.createEvent("Engine.Gui.CREATE_GUI_COMMAND", {
            id: guiId,
            layoutId,
        })
    );
    // Re-Activate GUI
    $services.commandService.send(
        $utils.createEvent("Engine.Gui.ACTIVATE_GUI_COMMAND", {
            id: guiId,
        })
    );
}
