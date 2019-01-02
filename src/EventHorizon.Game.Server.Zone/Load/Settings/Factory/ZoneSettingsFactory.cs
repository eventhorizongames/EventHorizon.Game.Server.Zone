
using System.Collections.Generic;
using EventHorizon.Game.Server.Zone.Load.Settings.Model;
using EventHorizon.Game.Server.Zone.Settings.Load;

namespace EventHorizon.Game.Server.Zone.Load.Settings.Factory
{
    public class ZoneSettingsFactory : IZoneSettingsFactory, IZoneSettingsSetter
    {
        private static ZoneSettings EMPTY_ZONE_SETTINGS = new ZoneSettings { Tag = "new" };
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