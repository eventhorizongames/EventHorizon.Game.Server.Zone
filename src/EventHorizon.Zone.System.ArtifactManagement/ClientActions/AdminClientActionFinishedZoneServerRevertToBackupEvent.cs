namespace EventHorizon.Zone.System.ArtifactManagement.ClientActions;

using EventHorizon.Zone.System.Admin.AdminClientAction.Generic;
using EventHorizon.Zone.System.Admin.AdminClientAction.Model;

public static class AdminClientActionFinishedZoneServerRevertToBackupEvent
{
    public static AdminClientActionGenericToAllEvent Create(
        string referenceId
    ) => new(
        "ZONE_SERVER_REVERT_TO_BACKUP_FINISHED_ADMIN_CLIENT_ACTION",
        new AdminClientActionFinishedZoneServerRevertToBackupEventData(
            referenceId
        )
    );

    private struct AdminClientActionFinishedZoneServerRevertToBackupEventData
        : IAdminClientActionData
    {
        public string ReferenceId { get; }

        public AdminClientActionFinishedZoneServerRevertToBackupEventData(
            string referenceId
        )
        {
            ReferenceId = referenceId;
        }
    }
}
