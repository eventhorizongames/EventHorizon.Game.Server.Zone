using EventHorizon.Zone.Core.Model.Client.DataType;
using MediatR;

namespace EventHorizon.Zone.Core.Events.Client.Actions
{
    public class ClientActionClientEntityStoppingToAllEvent : ClientActionToAllEvent<EntityClientStoppingData>, INotification
    {
        public override string Action => "ClientEntityStopping";
        public override EntityClientStoppingData Data { get; set; }
    }
}