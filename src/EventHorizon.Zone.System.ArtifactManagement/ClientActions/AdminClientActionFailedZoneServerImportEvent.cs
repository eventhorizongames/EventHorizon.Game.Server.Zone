namespace EventHorizon.Zone.System.ArtifactManagement.ClientActions;

using EventHorizon.Zone.System.Admin.AdminClientAction.Generic;
using EventHorizon.Zone.System.Admin.AdminClientAction.Model;

public static class AdminClientActionFailedZoneServerImportEvent
{
    public static AdminClientActionGenericToAllEvent Create(
        string referenceId,
        string errorCode
    ) => new(
        "ZONE_SERVER_IMPORT_FAILED_ADMIN_CLIENT_ACTION",
        new AdminClientActionFailedZoneServerImportEventData(
            referenceId,
            errorCode
        )
    );

    private struct AdminClientActionFailedZoneServerImportEventData
        : IAdminClientActionData
    {
        public string ReferenceId { get; }
        public string ErrorCode { get; }

        public AdminClientActionFailedZoneServerImportEventData(
            string referenceId,
            string errorCode
        )
        {
            ReferenceId = referenceId;
            ErrorCode = errorCode;
        }
    }
}
