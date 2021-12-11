namespace EventHorizon.Zone.System.ArtifactManagement.ClientActions;

using EventHorizon.Zone.System.Admin.AdminClientAction.Generic;
using EventHorizon.Zone.System.Admin.AdminClientAction.Model;

public static class ClientActionFinishedZoneServerImportEvent
{
    public static AdminClientActionGenericToAllEvent Create(
        string referenceId
    ) => new(
        "ZONE_SERVER_IMPORT_FINISHED_CLIENT_ACTION",
        new ClientActionFinishedZoneServerImportEventData(
            referenceId
        )
    );

    private struct ClientActionFinishedZoneServerImportEventData
        : IAdminClientActionData
    {
        public string ReferenceId { get; }

        public ClientActionFinishedZoneServerImportEventData(
            string referenceId
        )
        {
            ReferenceId = referenceId;
        }
    }
}
