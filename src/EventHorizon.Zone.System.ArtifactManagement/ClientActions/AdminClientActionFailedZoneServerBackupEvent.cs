namespace EventHorizon.Zone.System.ArtifactManagement.ClientActions;

using EventHorizon.Zone.System.Admin.AdminClientAction.Generic;
using EventHorizon.Zone.System.Admin.AdminClientAction.Model;

public static class AdminClientActionFailedZoneServerBackupEvent
{
    public static AdminClientActionGenericToAllEvent Create(
        string referenceId,
        string errorCode
    ) => new(
        "ZONE_SERVER_BACKUP_FAILED_ADMIN_CLIENT_ACTION",
        new AdminClientActionFailedZoneServerBackupEventData(
            referenceId,
            errorCode
        )
    );

    private struct AdminClientActionFailedZoneServerBackupEventData
        : IAdminClientActionData
    {
        public string ReferenceId { get; }
        public string ErrorCode { get; }

        public AdminClientActionFailedZoneServerBackupEventData(
            string referenceId,
            string errorCode
        )
        {
            ReferenceId = referenceId;
            ErrorCode = errorCode;
        }
    }
}
