namespace EventHorizon.Zone.Core.Events.SystemLog.Client
{
    using EventHorizon.Zone.Core.Events.Client.Generic;
    using EventHorizon.Zone.Core.Model.SystemLog.Client;

    public static class ClientActionMessageFromSystemToSingleEvent
    {
        public static ClientActionGenericToSingleEvent Create(
            string connectionId,
            MessageFromSystemData data
        ) => new ClientActionGenericToSingleEvent(
            connectionId,
            "SystemLog",
            data
        );
    }
}
