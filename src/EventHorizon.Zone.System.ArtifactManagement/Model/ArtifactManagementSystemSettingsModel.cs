namespace EventHorizon.Zone.System.ArtifactManagement;

using EventHorizon.Game.Server.Zone;

using global::System.Collections.Generic;

internal class ArtifactManagementSystemSettingsModel
    : ArtifactManagementSystemSettings
{
    public ICollection<string> AllowedDomainList { get; }

    public ArtifactManagementSystemSettingsModel(
        SystemArtifactManagementOptions options
    )
    {
        AllowedDomainList = options.AllowedDomainList;
    }
}
