using System.Collections.Generic;
using EventHorizon.Game.Server.Zone.Load.Model;

namespace EventHorizon.Game.Server.Zone.Load
{
    public interface IZoneSettingsFactory
    {
        ZoneSettings Settings { get; }
    }
}