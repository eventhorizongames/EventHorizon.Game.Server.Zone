namespace EventHorizon.Zone.System.ArtifactManagement.ClientActions;

using EventHorizon.Zone.System.Admin.AdminClientAction.Generic;
using EventHorizon.Zone.System.Admin.AdminClientAction.Model;

public static class AdminClientActionFinishedZoneServerImportEvent
{
    public static AdminClientActionGenericToAllEvent Create(
        string referenceId
    ) => new(
        "ZONE_SERVER_IMPORT_FINISHED_ADMIN_CLIENT_ACTION",
        new AdminClientActionFinishedZoneServerImportEventData(
            referenceId
        )
    );

    private struct AdminClientActionFinishedZoneServerImportEventData
        : IAdminClientActionData
    {
        public string ReferenceId { get; }

        public AdminClientActionFinishedZoneServerImportEventData(
            string referenceId
        )
        {
            ReferenceId = referenceId;
        }
    }
}
