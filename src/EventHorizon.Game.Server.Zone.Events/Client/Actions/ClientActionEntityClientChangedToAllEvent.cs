using EventHorizon.Game.Server.Zone.Client.DataType;
using EventHorizon.Zone.Core.Model.Client;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Events.Client.Actions
{
    public class ClientActionEntityClientChangedToAllEvent : ClientActionToAllEvent<EntityChangedData>, INotification
    {
        public override string Action => "EntityClientChanged";

        public override EntityChangedData Data { get; set; }
    }
}