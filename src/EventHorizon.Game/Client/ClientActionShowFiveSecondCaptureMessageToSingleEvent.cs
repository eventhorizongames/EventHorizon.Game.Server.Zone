using EventHorizon.Game.Model.Client;
using EventHorizon.Zone.Core.Events.Client.Generic;

namespace EventHorizon.Game.Client
{
    public static class ClientActionShowFiveSecondCaptureMessageToSingleEvent
    {
        public static ClientActionGenericToSingleEvent Create(
            string connectionId,
            ClientActionShowFiveSecondCaptureMessageData data
        ) => new ClientActionGenericToSingleEvent(
            connectionId,
            "Server.SHOW_FIVE_SECOND_CAPTURE_MESSAGE",
            data
        );
    }
}
