/**
 * Action Id: un_freeze_entity
 * 
 * Services:
 * - EntityRepository
 * Data: 
 * - id: string
 */

const entity = Services.EntityRepository.get(
    Data.id
);
if (entity == undefined) {
    return;
}