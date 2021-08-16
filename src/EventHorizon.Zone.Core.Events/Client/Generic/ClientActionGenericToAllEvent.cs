namespace EventHorizon.Zone.Core.Events.Client.Generic
{
    using EventHorizon.Zone.Core.Model.Client;

    using MediatR;

    public class ClientActionGenericToAllEvent : ClientActionToAllEvent<IClientActionData>, INotification
    {
        public override string Action { get; }
        public override IClientActionData Data { get; }

        public ClientActionGenericToAllEvent(
            string action,
            IClientActionData data
        )
        {
            Action = action;
            Data = data;
        }
    }
}
