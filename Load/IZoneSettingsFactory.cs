using System.Collections.Generic;

namespace EventHorizon.Game.Server.Zone.Load
{
    public interface IZoneSettingsFactory
    {
        IZoneSettings Settings { get; }
    }
}