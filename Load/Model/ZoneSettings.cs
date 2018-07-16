using System.Collections.Generic;

namespace EventHorizon.Game.Server.Zone.Load.Model
{
    public class ZoneSettings : IZoneSettings
    {
        public IList<string> Tags { get; set; }
        public int MapDimensions { get; set; }
        public int TileDimension { get; set; }
    }
}