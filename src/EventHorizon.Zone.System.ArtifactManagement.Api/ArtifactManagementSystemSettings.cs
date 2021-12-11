namespace EventHorizon.Zone.System.ArtifactManagement;

using global::System.Collections.Generic;

public interface ArtifactManagementSystemSettings
{
    ICollection<string> AllowedDomainList { get; }
}
