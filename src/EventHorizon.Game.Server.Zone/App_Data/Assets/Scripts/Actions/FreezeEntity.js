/** 
 * Action Id: freeze_entity
 * 
 * $services:
 * - EntityRepository
 * $data:
 * - id: string
 */

const entity = $services.entityRepository.get(
    $data.id
);
if (entity == undefined) {
    return;
}
// TODO: Block Client side movement, this is good so bad movement requests do not get propagated to server.