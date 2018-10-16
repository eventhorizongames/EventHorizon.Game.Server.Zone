using EventHorizon.Game.Server.Zone.Client.DataType;
using EventHorizon.Game.Server.Zone.Model.Client;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Events.Client.Actions
{
    public class ClientActionEntityRegisteredEvent : ClientActionEvent<EntityRegisteredData>, INotification
    {
        public override string Action => "EntityRegistered";

        public override EntityRegisteredData Data { get; set; }
    }
}