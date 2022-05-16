using ClientEvents = EventHorizon.Zone.Core.Events.Client.Generic;

public static class ClientActionShowTenSecondCaptureMessageToSingleEvent
{
    public static ClientEvents.ClientActionGenericToSingleEvent Create(
        string connectionId,
        ClientActionShowTenSecondCaptureMessageData data
    ) => new(connectionId, "Server.SHOW_TEN_SECOND_CAPTURE_MESSAGE", data);
}
