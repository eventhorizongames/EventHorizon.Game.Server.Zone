namespace EventHorizon.Zone.System.ArtifactManagement.ClientActions;

using EventHorizon.Zone.System.Admin.AdminClientAction.Generic;
using EventHorizon.Zone.System.Admin.AdminClientAction.Model;

public static class ClientActionFinishedZoneServerRevertToBackupEvent
{
    public static AdminClientActionGenericToAllEvent Create(
        string referenceId
    ) => new(
        "ZONE_SERVER_REVERT_TO_BACKUP_FINISHED_CLIENT_ACTION",
        new ClientActionFinishedZoneServerRevertToBackupEventData(
            referenceId
        )
    );

    private struct ClientActionFinishedZoneServerRevertToBackupEventData
        : IAdminClientActionData
    {
        public string ReferenceId { get; }

        public ClientActionFinishedZoneServerRevertToBackupEventData(
            string referenceId
        )
        {
            ReferenceId = referenceId;
        }
    }
}
