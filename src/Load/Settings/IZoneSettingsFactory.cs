using EventHorizon.Game.Server.Zone.Load.Settings.Model;

namespace EventHorizon.Game.Server.Zone.Settings.Load
{
    public interface IZoneSettingsFactory
    {
        ZoneSettings Settings { get; }
    }
}