using EventHorizon.Zone.Core.Model.Client.DataType;
using MediatR;

namespace EventHorizon.Zone.Core.Events.Client.Actions
{
    public class ClientActionEntityClientMoveToAllEvent : ClientActionToAllEvent<EntityClientMoveData>, INotification
    {
        public override string Action => "EntityClientMove";

        public override EntityClientMoveData Data { get; set; }
    }
}