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
function guiControlId() {
    return `HealthModule_${$entity.entityId}`;
}
function guiLayoutId() {
    return `${guiControlId()}-layout`;
}
function onEntityChanged({ entityId }) {
    if ($entity.entityId !== entityId) {
        return;
    }
    $services.logger.debug("onEntityChnaged", {
        entityId: $entity.entityId,
        passedEntityId: entityId,
        options: {
            text: getEntityText(),
            percent: getEntityPercent(),
        },
    });

    $services.commandService.send({
        type: {
            key: "Engine.Gui.UPDATE_GUI_CONTROL_COMMAND",
        },
        data: {
            controlId: guiControlId(),
            options: {
                text: getEntityText(),
                percent: getEntityPercent(),
            },
        },
    });
}
function getEntityText() {
    const lifeState = $entity.getProperty("lifeState");
    return `${lifeState.healthPoints}/${lifeState.maxHealthPoints}`;
}
function getEntityPercent() {
    const lifeState = $entity.getProperty("lifeState");
    return (lifeState.healthPoints / lifeState.maxHealthPoints) * 100;
}
function onMeshSet({ id }) {
    if ($entity.id !== id) {
        return;
    }

    $services.commandService.send({
        type: {
            key: "GUI.LINK_GUI_CONTROL_WITH_MESH_COMMAND",
        },
        data: {
            controlId: guiControlId(),
            mesh: $entity.getProperty("MESH_MODULE_NAME").mesh,
        },
    });
}

// Setup Event Listener's
$services.eventService.addEventListener(
    {
        key: "Entity.ENTITY_CHANGED_SUCCESSFULLY_EVENT",
    },
    onEntityChanged,
    this
);
$services.eventService.addEventListener(
    {
        key: "Module.Mesh.MESH_SET_EVENT",
    },
    onMeshSet,
    this
);

// Add the guiControlId to the data to signal when it is disposed.
$data.guiControlId = guiControlId();

// Add eventsToRemove to the $data, will be called by the Dispose script
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

$services.commandService.send({
    type: {
        key: "GUI.ADD_LAYOUT_COMMAND",
    },
    data: {
        layout: {
            id: guiLayoutId(),
            sort: 0,
            controlList: [
                {
                    id: guiControlId(),
                    sort: 0,
                    controlList: [],
                },
            ],
        },
    },
});

// Register the control with the provided template and options.
$services.commandService.send({
    type: {
        key: "GUI.REGISTER_CONTROL_COMMAND",
    },
    data: {
        controlId: guiControlId(),
        templateId: "HealthModule-Template",
        options: {
            text: getEntityText(),
            percent: getEntityPercent(),
        },
    },
});

// Activate the layout
$services.commandService.send({
    type: {
        key: "GUI.ACTIVATE_LAYOUT_COMMAND",
    },
    data: {
        layoutId: guiLayoutId(),
    },
});

// Link the GUI to the $entity mesh
$services.commandService.send({
    type: {
        key: "GUI.LINK_GUI_CONTROL_WITH_MESH_COMMAND",
    },
    data: {
        controlId: guiControlId(),
        mesh: $entity.getProperty("MESH_MODULE_NAME").mesh,
    },
});
