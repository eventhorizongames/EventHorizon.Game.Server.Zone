using EventHorizon.Game.Server.Zone.Client.DataType;
using EventHorizon.Game.Server.Zone.Model.Client;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Events.Client.Actions
{
    public class ClientActionEntityClientMoveEvent : ClientActionEvent<EntityClientMoveData>, INotification
    {
        public override string Action => "EntityClientMove";

        public override EntityClientMoveData Data { get; set; }
    }
}