using EventHorizon.Game.Server.Zone.Client.DataType;
using EventHorizon.Game.Server.Zone.Model.Client;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Events.Client.Actions
{
    public class ClientActionEntityClientChangedEvent : ClientActionEvent<EntityChangedData>, INotification
    {
        public override string Action => "EntityClientChanged";

        public override EntityChangedData Data { get; set; }
    }
}