/**
 * $services: {
 *  logger: ILogger;
 *  i18n: II18nMap;
 *  eventService: IEventService;
 *  commandService: ICommandService;
 *  queryService: IQueryService;
 *  renderingApi: IEngineRenderingAPI;
 * }
 * $state: {
 *  isObjectDefined(obj: any): boolean;
 *  createEvent(event: string, data?: any): IEvent;
 *  createQuery(
 *      queryType: string,
 *      data?: any
 *  ): IQuery<unknown, unknown>;
 *  runClientScript(id: string, name: string, data: any): void;
 *  resolveTemplate(
 *      messageTemplate: string,
 *      templateData: any
 *  ): string;
 *  sendEvent(eventName: string, data: any): void;
 *  translation(key: string, data: any): string;
 * }
 * Events Used:
 * - Client.Game.Updated.GAME_STATE_UPDATED
 * - Client.Game.Query.CURRENT_GAME_STATE
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

// Setup Event Listeners
$services.eventService.on(
    {
        key: "Client.Game.Updated.GAME_STATE_UPDATED",
    },
    handleGameStateChanged,
    this
);

// Add Events to the $data, that can be disposed of later
$data.eventsToRemove = [];
$data.eventsToRemove.push({
    name: "Client.Game.Updated.GAME_STATE_UPDATED",
    handler: handleGameStateChanged,
    context: this,
});

// Script Logic
const createEntityIdTag = (entityId) => `id:${entityId}`;

handleGameStateChanged();
function handleGameStateChanged(_ /* {} */) {
    var gameState = $services.queryService.query(
        $utils.createQuery("Client.Game.Query.CURRENT_GAME_STATE")
    ).result;
    // Remove currently existing GUI
    $services.commandService.send(
        $utils.createEvent("Engine.Gui.DISPOSE_OF_GUI_COMMAND", {
            id: guiId,
        })
    );

    // Create and Active new GUI
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

    // Create New List of Player Scores GUI Controls
    const newEntryLayoutId = "GUI_LeaderBoard-Entry.json";
    console.log("LeaderBoard Setup", { gameState });
    // GET Player List
    const playerList = gameState.scores.map(({ playerEntityId, score }) => ({
        player: $services.queryService.query(
            $utils.createQuery("Entity.Tracked.QUERY_FOR_ENTITY", {
                tag: createEntityIdTag(playerEntityId),
            })
        ).result[0],
        score,
    }));
    // const newMessageGuiId = `GUI_LeaderBoard-Entry_${playerEntityId}`;
    // Send Update to GUI Control
    playerList.forEach((playerScore) => {
        console.log({ playerScore });
        const newGuiId = `GUI_LeaderBoard-Entry_${playerScore.player.entityId}`;
        // Remove currently existing GUI
        $services.commandService.send(
            $utils.createEvent("Engine.Gui.DISPOSE_OF_GUI_COMMAND", {
                id: newGuiId,
            })
        );
        $services.commandService.send(
            $utils.createEvent("Engine.Gui.CREATE_GUI_COMMAND", {
                id: newGuiId,
                layoutId: newEntryLayoutId,
                parentControlId: $services.queryService.query({
                    type: "Engine.Gui.QUERY_FOR_GENERATE_GUI_CONTROL_ID",
                    data: {
                        guiId,
                        controlId: "leaderboard-panel",
                    },
                }).result,
                controlDataList: [
                    {
                        controlId: "leaderboard-entry",
                        options: {
                            // TODO: translate/localize this text
                            text: `Player [${playerScore.player.name}]: ${playerScore.score}`,
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
    console.log("LeaderBoard Setup", {
        gameState,
        playerList,
        rootControl: $services.queryService.query({
            type: "Engine.Gui.QUERY_FOR_GENERATE_GUI_CONTROL_ID",
            data: {
                guiId,
                controlId: "leaderboard-panel",
            },
        }).result,
    });
}

let isOpen = false;
function HandleOpenOfLeaderBoard() {
    if (isOpen) {
        return;
    }
    isOpen = true;
    // Show the LeaderBoard Window
    $services.commandService.send(
        $utils.createEvent("Engine.Gui.SHOW_GUI_COMMAND", {
            id: guiId,
        })
    );
}

function HandleCloseOfLeaderBoard() {
    if (!isOpen) {
        return;
    }
    isOpen = false;
    // Hide the LeaderBoard Window
    $services.commandService.send(
        $utils.createEvent("Engine.Gui.HIDE_GUI_COMMAND", {
            id: guiId,
        })
    );
}

// Register Input for LeaderBoard Keyboard Shortcut
// Current [t] key
$data.leaderBoardKeyboardShortcut = {
    key: "t",
    pressed: HandleOpenOfLeaderBoard,
    released: HandleCloseOfLeaderBoard,
};
$services.commandService.send(
    $utils.createEvent(
        "Engine.Input.REGISTER_INPUT_COMMAND",
        $data.leaderBoardKeyboardShortcut
    )
);
