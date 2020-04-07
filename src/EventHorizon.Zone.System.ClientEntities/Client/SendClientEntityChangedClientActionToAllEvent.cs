namespace EventHorizon.Zone.System.ClientEntities.Client
{
    using EventHorizon.Zone.Core.Events.Client;
    using EventHorizon.Zone.System.ClientEntities.Model.Client;
    using MediatR;

    public class SendClientEntityChangedClientActionToAllEvent : ClientActionToAllEvent<ClientEntityChangedClientActionData>, INotification
    {
        public override string Action => "SERVER_CLIENT_ENTITY_CHANGED_CLIENT_ACTION_EVENT";
        public override ClientEntityChangedClientActionData Data { get; set; }

        public SendClientEntityChangedClientActionToAllEvent(
            ClientEntityChangedClientActionData data
        )
        {
            Data = data;
        }
    }
}