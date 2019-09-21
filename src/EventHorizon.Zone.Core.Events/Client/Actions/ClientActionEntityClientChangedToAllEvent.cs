using EventHorizon.Zone.Core.Model.Client.DataType;
using MediatR;

namespace EventHorizon.Zone.Core.Events.Client.Actions
{
    public class ClientActionEntityClientChangedToAllEvent : ClientActionToAllEvent<EntityChangedData>, INotification
    {
        public override string Action => "EntityClientChanged";

        public override EntityChangedData Data { get; set; }
    }
}