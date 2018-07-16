using System.Collections.Generic;
using EventHorizon.Game.Server.Zone.Load.Model;

namespace EventHorizon.Game.Server.Zone.Load
{
    public interface IZoneSettingsSetter
    {
        void Set(ZoneSettings zoneSettings);
    }
}