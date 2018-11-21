// Services:
// - EntityRepository
// Data: 
// - id: string

const entity = Services.EntityRepository.get(
    Data.id
);
if (entity == undefined) {
    return;
}
// TODO: Block Client side movement, this is good so bad movement requests do not get propagated to server.