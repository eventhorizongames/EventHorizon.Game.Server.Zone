namespace EventHorizon.Zone.System.ArtifactManagement.ClientActions;

using EventHorizon.Zone.System.Admin.AdminClientAction.Generic;
using EventHorizon.Zone.System.Admin.AdminClientAction.Model;

public static class AdminClientActionFailedZoneServerExportEvent
{
    public static AdminClientActionGenericToAllEvent Create(
        string referenceId,
        string errorCode
    ) => new(
        "ZONE_SERVER_EXPORT_FAILED_ADMIN_CLIENT_ACTION",
        new AdminClientActionFailedZoneServerExportEventData(
            referenceId,
            errorCode
        )
    );

    private struct AdminClientActionFailedZoneServerExportEventData
        : IAdminClientActionData
    {
        public string ReferenceId { get; }
        public string ErrorCode { get; }

        public AdminClientActionFailedZoneServerExportEventData(
            string referenceId,
            string errorCode
        )
        {
            ReferenceId = referenceId;
            ErrorCode = errorCode;
        }
    }
}
