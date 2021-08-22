namespace EventHorizon.Game.Server.Zone.Settings.Load
{
    using EventHorizon.Zone.Core.Model.Settings;

    public interface IZoneSettingsSetter
    {
        void Set(
            ZoneSettings zoneSettings
        );
    }
}
