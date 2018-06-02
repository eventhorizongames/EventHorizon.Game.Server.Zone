using System.Collections.Generic;

namespace EventHorizon.Game.Server.Zone.Core.Model
{
    public class ZoneSettings
    {
        public IList<string> Tags { get; set; }
        public int MapDimensions { get; set; }
        public int TileDimension { get; set; }
    }
}