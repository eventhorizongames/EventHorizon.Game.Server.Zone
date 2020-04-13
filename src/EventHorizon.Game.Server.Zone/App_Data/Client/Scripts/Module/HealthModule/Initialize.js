/**
 *
 * $services: {
 *  eventService: {
 *      publish: (event: IEvent)
 *  }
 *  commandService: {
 *      send: (command: ICommand)
 *  }
 * }
 *
 * This is the IObjectEntity that this module is attached to
 * $entity: {
 *  id: number
 * }
 *
 * This the internal "state" of the script, only accessible by the script.
 * $state: {
 * };
 *
 * This is data passed to the script from the outside.
 * $data: {
 *  $entity: IObjectEntity
 *  eventsToDispose: IEventToDispose
 * };
 */
const { $entity } = $data;
const layoutId = "GUI_Module_HealthModule.json";
$services.logger.debug("HealthModule_Initialize");

$data.guiId = `health_module-${$entity.entityId}`;

$services.commandService.send(
    $utils.createEvent("Engine.Gui.CREATE_GUI_COMMAND", {
        id: $data.guiId,
        layoutId,
        controlDataList: [
            {
                controlId: "gui_health_module-bar",
                options: {
                    text: getEntityText(),
                    percent: getEntityPercent(),
                },
                linkWith: $entity.getProperty("MESH_MODULE_NAME").mesh,
            },
        ],
    })
);
$services.commandService.send(
    $utils.createEvent("Engine.Gui.ACTIVATE_GUI_COMMAND", {
        id: $data.guiId,
    })
);

function onEntityChanged({ entityId }) {
    console.log("HealthModuleInitialize", { entityId });
    if ($entity.entityId !== entityId) {
        return;
    }
    $services.logger.debug("onEntityChanged", {
        entityId: $entity.entityId,
        passedEntityId: entityId,
        options: {
            text: getEntityText(),
            percent: getEntityPercent(),
        },
    });

    $services.commandService.send(
        $utils.createEvent("Engine.Gui.UPDATE_GUI_CONTROL_COMMAND", {
            guiId: $data.guiId,
            control: {
                controlId: "gui_health_module-bar",
                options: {
                    text: getEntityText(),
                    percent: getEntityPercent(),
                },
            },
        })
    );
}
function getEntityText() {
    const lifeState = $entity.getProperty("lifeState");
    return `${lifeState.healthPoints}/${lifeState.maxHealthPoints}`;
}
function getEntityPercent() {
    const lifeState = $entity.getProperty("lifeState");
    return (lifeState.healthPoints / lifeState.maxHealthPoints) * 100;
}
function onMeshSet({ clientId }) {
    console.log("HealthModuleInitialize", { $entity, clientId });
    if ($entity.clientId !== clientId) {
        return;
    }

    $services.commandService.send(
        $utils.createEvent("Engine.Gui.UPDATE_GUI_CONTROL_COMMAND", {
            guiId: $data.guiId,
            control: {
                controlId: "gui_health_module-bar",
                linkWith: $entity.getProperty("MESH_MODULE_NAME").mesh,
            },
        })
    );
}

// Setup Event Listener's
$services.eventService.on(
    {
        key: "Entity.ENTITY_CHANGED_SUCCESSFULLY_EVENT",
    },
    onEntityChanged,
    this
);
$services.eventService.on(
    {
        key: "Module.Mesh.MESH_SET_EVENT",
    },
    onMeshSet,
    this
);
$data.eventsToRemove = [];
$data.eventsToRemove.push({
    name: "Entity.ENTITY_CHANGED_SUCCESSFULLY_EVENT",
    handler: onEntityChanged,
    context: this,
});
$data.eventsToRemove.push({
    name: "Module.Mesh.MESH_SET_EVENT",
    handler: onMeshSet,
    context: this,
});
