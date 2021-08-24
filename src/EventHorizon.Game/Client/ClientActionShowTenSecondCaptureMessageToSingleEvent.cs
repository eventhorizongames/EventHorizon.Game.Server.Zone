namespace EventHorizon.Game.Client
{
    using EventHorizon.Game.Model.Client;
    using EventHorizon.Zone.Core.Events.Client.Generic;

    public static class ClientActionShowTenSecondCaptureMessageToSingleEvent
    {
        public static ClientActionGenericToSingleEvent Create(
            string connectionId,
            ClientActionShowTenSecondCaptureMessageData data
        ) => new(
            connectionId,
            "Server.SHOW_TEN_SECOND_CAPTURE_MESSAGE",
            data
        );
    }
}
