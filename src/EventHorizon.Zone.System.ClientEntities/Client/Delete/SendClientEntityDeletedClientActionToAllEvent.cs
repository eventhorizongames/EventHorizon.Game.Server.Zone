namespace EventHorizon.Zone.System.ClientEntities.Client.Delete
{
    using EventHorizon.Zone.Core.Events.Client;
    using EventHorizon.Zone.System.ClientEntities.Model.Client;
    using MediatR;

    public class SendClientEntityDeletedClientActionToAllEvent : ClientActionToAllEvent<ClientEntityDeletedClientActionData>, INotification
    {
        public override string Action => "SERVER_CLIENT_ENTITY_DELETED_CLIENT_ACTION_EVENT";
        public override ClientEntityDeletedClientActionData Data { get; set; }

        public SendClientEntityDeletedClientActionToAllEvent(
            ClientEntityDeletedClientActionData data
        )
        {
            Data = data;
        }
    }
}