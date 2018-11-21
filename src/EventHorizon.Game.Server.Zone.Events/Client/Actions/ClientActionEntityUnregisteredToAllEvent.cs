using EventHorizon.Game.Server.Zone.Client.DataType;
using EventHorizon.Game.Server.Zone.Model.Client;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Events.Client.Actions
{
    public class ClientActionEntityUnregisteredToAllEvent : ClientActionToAllEvent<EntityUnregisteredData>, INotification
    {
        public override string Action => "EntityUnregistered";

        public override EntityUnregisteredData Data { get; set; }
    }
}