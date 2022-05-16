namespace EventHorizon.Game.Server.Zone;

using System.Collections.Generic;

using EventHorizon.Zone.System.ArtifactManagement.Model;

public struct SystemArtifactManagementOptions
{
    public List<string> AllowedDomainList { get; set; } =
        new List<string> { ArtifactManagementSystemContants.ALL_DOMAINS, };

    public SystemArtifactManagementOptions(List<string> allowedDomainList)
    {
        AllowedDomainList = allowedDomainList;
    }
}
