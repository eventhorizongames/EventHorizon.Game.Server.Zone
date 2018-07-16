using System.Collections.Generic;
using EventHorizon.Game.Server.Zone.Load.Model;

namespace EventHorizon.Game.Server.Zone.Load.Factory
{
    public class ZoneSettingsFactory : IZoneSettingsFactory, IZoneSettingsSetter
    {
        private static ZoneSettings EMPTY_ZONE_SETTINGS = new ZoneSettings { Tags = new List<string>() { "new" } };
        public ZoneSettings Settings { get; private set; }

        public ZoneSettingsFactory()
        {
            this.Settings = EMPTY_ZONE_SETTINGS;
        }

        public void Set(ZoneSettings settings)
        {
            Settings = settings;
        }
    }
}