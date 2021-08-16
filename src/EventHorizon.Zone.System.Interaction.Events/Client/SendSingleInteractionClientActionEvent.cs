namespace EventHorizon.Zone.System.Interaction.Events.Client
{
    using EventHorizon.Zone.Core.Events.Client.Generic;
    using EventHorizon.Zone.System.Interaction.Model.Client;

    public static class SendSingleInteractionClientActionEvent
    {
        public static ClientActionGenericToSingleEvent Create(
            string connectionId,
            InteractionClientActionData data
        ) => new ClientActionGenericToSingleEvent(
            connectionId,
            "SERVER_INTERACTION_CLIENT_ACTION_EVENT",
            data
        );
    }
}
