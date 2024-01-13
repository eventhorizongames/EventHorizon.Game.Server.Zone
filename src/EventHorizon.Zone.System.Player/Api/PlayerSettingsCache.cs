namespace EventHorizon.Zone.System.Player.Api;

using EventHorizon.Zone.Core.Model.Entity;


public interface PlayerSettingsCache
{
    ObjectEntityConfiguration PlayerConfiguration { get; }
    ObjectEntityData PlayerData { get; }
}
