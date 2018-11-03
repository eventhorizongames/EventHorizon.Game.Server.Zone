// Services:
// - EntityRepository
// Data: 
// - id: EntityId

const entity = Services.EntityRepository.get(
    Data.id
);
if (entity == undefined) {
    return;
}
const moveModule = entity.getProperty("MOVE_MODULE_NAME");
moveModule.enabled = false;