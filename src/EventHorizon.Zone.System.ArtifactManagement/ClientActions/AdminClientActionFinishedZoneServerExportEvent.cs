namespace EventHorizon.Zone.System.ArtifactManagement.ClientActions;

using EventHorizon.Zone.System.Admin.AdminClientAction.Generic;
using EventHorizon.Zone.System.Admin.AdminClientAction.Model;
public static class AdminClientActionFinishedZoneServerExportEvent
{
    public static AdminClientActionGenericToAllEvent Create(
        string referenceId,
        string exportPath
    ) => new(
        "ZONE_SERVER_EXPORT_FINISHED_ADMIN_CLIENT_ACTION",
        new AdminClientActionFinishedZoneServerExportEventData(
            referenceId,
            exportPath
        )
    );

    private struct AdminClientActionFinishedZoneServerExportEventData
        : IAdminClientActionData
    {
        public string ReferenceId { get; }
        public string ExportPath { get; }

        public AdminClientActionFinishedZoneServerExportEventData(
            string referenceId,
            string exportPath
        )
        {
            ReferenceId = referenceId;
            ExportPath = exportPath;
        }
    }
}
