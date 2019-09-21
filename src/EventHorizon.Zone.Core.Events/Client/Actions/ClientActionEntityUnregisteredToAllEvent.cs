using EventHorizon.Zone.Core.Model.Client.DataType;
using MediatR;

namespace EventHorizon.Zone.Core.Events.Client.Actions
{
    public class ClientActionEntityUnregisteredToAllEvent : ClientActionToAllEvent<EntityUnregisteredData>, INotification
    {
        public override string Action => "EntityUnregistered";

        public override EntityUnregisteredData Data { get; set; }
    }
}