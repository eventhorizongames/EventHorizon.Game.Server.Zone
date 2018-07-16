using System.Collections.Generic;

namespace EventHorizon.Game.Server.Zone.Load
{
    public interface IZoneSettingsSetter
    {
        void Set(IZoneSettings zoneSettings);
    }
}