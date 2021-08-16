using EventHorizon.Zone.Core.Model.Settings;

namespace EventHorizon.Game.Server.Zone.Settings.Load
{
    public interface IZoneSettingsSetter
    {
        void Set(
            ZoneSettings zoneSettings
        );
    }
}
