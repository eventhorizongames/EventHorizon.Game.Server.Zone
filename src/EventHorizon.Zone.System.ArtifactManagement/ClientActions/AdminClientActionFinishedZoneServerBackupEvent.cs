namespace EventHorizon.Zone.System.ArtifactManagement.ClientActions;

using EventHorizon.Zone.System.Admin.AdminClientAction.Generic;
using EventHorizon.Zone.System.Admin.AdminClientAction.Model;

public static class AdminClientActionFinishedZoneServerBackupEvent
{
    public static AdminClientActionGenericToAllEvent Create(
        string referenceId,
        string backupUrl
    ) => new(
        "ZONE_SERVER_BACKUP_FINISHED_ADMIN_CLIENT_ACTION",
        new AdminClientActionFinishedZoneServerBackupEventData(
            referenceId,
            backupUrl
        )
    );

    private struct AdminClientActionFinishedZoneServerBackupEventData
        : IAdminClientActionData
    {
        public string ReferenceId { get; }
        public string BackupUrl { get; }

        public AdminClientActionFinishedZoneServerBackupEventData(
            string referenceId,
            string backupUrl
        )
        {
            ReferenceId = referenceId;
            BackupUrl = backupUrl;
        }
    }
}
