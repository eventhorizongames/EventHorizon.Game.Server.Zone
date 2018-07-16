using System.Collections.Generic;

namespace EventHorizon.Game.Server.Zone.Load
{
    public interface IZoneSettings
    {
        IList<string> Tags { get; set; }
        int MapDimensions { get; set; }
        int TileDimension { get; set; }
    }
}