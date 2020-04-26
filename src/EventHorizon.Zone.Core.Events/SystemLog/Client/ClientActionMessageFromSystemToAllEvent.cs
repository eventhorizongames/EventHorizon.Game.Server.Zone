namespace EventHorizon.Zone.Core.Events.SystemLog.Client
{
    using EventHorizon.Zone.Core.Events.Client.Generic;
    using EventHorizon.Zone.Core.Model.SystemLog.Client;

    public static class ClientActionMessageFromSystemToAllEvent
    {
        public static ClientActionGenericToAllEvent Create(
            MessageFromSystemData data
        ) => new ClientActionGenericToAllEvent(
            "MessageFromSystem",
            data
        );
    }
}
