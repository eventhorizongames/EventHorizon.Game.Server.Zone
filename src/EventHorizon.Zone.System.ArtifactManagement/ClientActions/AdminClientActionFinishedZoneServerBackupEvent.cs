namespace EventHorizon.Zone.System.ArtifactManagement.ClientActions;

using EventHorizon.Zone.System.Admin.AdminClientAction.Generic;
using EventHorizon.Zone.System.Admin.AdminClientAction.Model;

public static class AdminClientActionFinishedZoneServerBackupEvent
{
    public static AdminClientActionGenericToAllEvent Create(
        string referenceId,
        string backupPath
    ) => new(
        "ZONE_SERVER_BACKUP_FINISHED_ADMIN_CLIENT_ACTION",
        new AdminClientActionFinishedZoneServerBackupEventData(
            referenceId,
            backupPath
        )
    );

    private struct AdminClientActionFinishedZoneServerBackupEventData
        : IAdminClientActionData
    {
        public string ReferenceId { get; }
        public string BackupPath { get; }

        public AdminClientActionFinishedZoneServerBackupEventData(
            string referenceId,
            string backupPath
        )
        {
            ReferenceId = referenceId;
            BackupPath = backupPath;
        }
    }
}
