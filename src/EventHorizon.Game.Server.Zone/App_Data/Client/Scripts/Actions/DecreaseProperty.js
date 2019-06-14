/*
 * Action Id: decrease_property
 *
 * $services:
 * - EntityRepository
 * $utils:
 * - sendEvent(eventName: string, data?: any)
 * $data:
 * - id: EntityId
 * - propertyName: string
 * - valueProperty: string
 * - amount: number
 */

const entity = $services.entityRepository.get($data.id);
if (entity == undefined) {
    return;
}
const property = entity.getProperty($data.propertyName);
property[$data.valueProperty] = property[$data.valueProperty] - $data.amount;
entity.setProperty($data.propertyName, property);

$utils.sendEvent("Entity.ENTITY_CHANGED_SUCCESSFULLY_EVENT", {
    entityId: $data.id
});
