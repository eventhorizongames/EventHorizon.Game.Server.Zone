// Action Id: decrease_property
// Services:
// - EntityRepository
// Methods:
// - sendEvent(eventName: string, data?: any)
// Data: 
// - id: EntityId
// - propertyName: string
// - valueProperty: string
// - amount: number

const entity = Services.EntityRepository.get(
    Data.id
);
if (entity == undefined) {
    return;
}
const property = entity.getProperty(Data.propertyName);
property[Data.valueProperty] = property[Data.valueProperty] - Data.amount;
entity.setProperty(Data.propertyName, property);

Methods.sendEvent(
    "ZONE.ENTITY_CHANGED_SUCCESSFULLY_EVENT",
    Data.id
);