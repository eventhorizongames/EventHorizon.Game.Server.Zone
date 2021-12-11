namespace EventHorizon.Zone.System.ArtifactManagement.ClientActions;

using EventHorizon.Zone.System.Admin.AdminClientAction.Generic;
using EventHorizon.Zone.System.Admin.AdminClientAction.Model;

public static class AdminClientActionFailedZoneServerRevertToBackupEvent
{
    public static AdminClientActionGenericToAllEvent Create(
        string referenceId,
        string errorCode
    ) => new(
        "ZONE_SERVER_REVERT_TO_BACKUP_FAILED_ADMIN_CLIENT_ACTION",
        new AdminClientActionFailedZoneServerRevertToBackupEventData(
            referenceId,
            errorCode
        )
    );

    private struct AdminClientActionFailedZoneServerRevertToBackupEventData
        : IAdminClientActionData
    {
        public string ReferenceId { get; }
        public string ErrorCode { get; }

        public AdminClientActionFailedZoneServerRevertToBackupEventData(
            string referenceId,
            string errorCode
        )
        {
            ReferenceId = referenceId;
            ErrorCode = errorCode;
        }
    }
}
