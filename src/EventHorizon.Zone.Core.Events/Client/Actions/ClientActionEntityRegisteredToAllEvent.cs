using EventHorizon.Zone.Core.Model.Client.DataType;
using MediatR;

namespace EventHorizon.Zone.Core.Events.Client.Actions
{
    public class ClientActionEntityRegisteredToAllEvent : ClientActionToAllEvent<EntityRegisteredData>, INotification
    {
        public override string Action => "EntityRegistered";

        public override EntityRegisteredData Data { get; set; }
    }
}