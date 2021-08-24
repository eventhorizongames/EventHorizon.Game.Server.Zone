namespace EventHorizon.Game.Server.Zone.Load.Settings.Factory
{
    using EventHorizon.Game.Server.Zone.Settings.Load;
    using EventHorizon.Zone.Core.Model.Settings;

    public class ZoneSettingsFactory
        : IZoneSettingsFactory,
        IZoneSettingsSetter
    {
        private static readonly ZoneSettings EMPTY_ZONE_SETTINGS = new() { Tag = "home", BaseMovementTimeOffset = 100, };

        public ZoneSettings Settings { get; private set; } = EMPTY_ZONE_SETTINGS;

        public void Set(
            ZoneSettings settings
        )
        {
            Settings = settings;
        }
    }
}
