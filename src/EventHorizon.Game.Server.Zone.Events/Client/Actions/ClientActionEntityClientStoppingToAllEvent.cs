using EventHorizon.Game.Server.Zone.Client.DataType;
using EventHorizon.Game.Server.Zone.Model.Client;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Events.Client.Actions
{
    public class ClientActionClientEntityStoppingToAllEvent : ClientActionToAllEvent<EntityClientStoppingData>, INotification
    {
        public override string Action => "ClientEntityStopping";
        public override EntityClientStoppingData Data { get; set; }
    }
}