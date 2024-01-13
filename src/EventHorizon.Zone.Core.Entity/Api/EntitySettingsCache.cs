namespace EventHorizon.Zone.Core.Entity.Api;

using EventHorizon.Zone.Core.Model.Entity;

public interface EntitySettingsCache
{
    ObjectEntityConfiguration EntityConfiguration { get; }
    ObjectEntityData EntityData { get; }
}
