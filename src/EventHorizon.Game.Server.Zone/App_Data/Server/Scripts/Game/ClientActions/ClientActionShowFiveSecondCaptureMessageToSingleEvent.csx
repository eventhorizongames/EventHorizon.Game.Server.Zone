using ClientEvents = EventHorizon.Zone.Core.Events.Client.Generic;

public static class ClientActionShowFiveSecondCaptureMessageToSingleEvent
{
    public static ClientEvents.ClientActionGenericToSingleEvent Create(
        string connectionId,
        ClientActionShowFiveSecondCaptureMessageData data
    ) => new(connectionId, "Server.SHOW_FIVE_SECOND_CAPTURE_MESSAGE", data);
}
