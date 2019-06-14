/**
 * Action Id: un_freeze_entity
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