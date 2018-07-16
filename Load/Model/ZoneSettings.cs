using System.Collections.Generic;

namespace EventHorizon.Game.Server.Zone.Load.Model
{
    public class ZoneSettings
    {
        public IList<string> Tags { get; set; }
        public ZoneMapSettings Map { get; set; }
    }
    public class ZoneMapSettings
    {
        public int Dimensions { get; set; }
        public int TileDimensions { get; set; }
        public ZoneMapMeshSettings Mesh { get; set; }
    }
    public class ZoneMapMeshSettings
    {
        public string HeightMapUrl { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Subdivisions { get; set; }
        public int MinHeight { get; set; }
        public int MaxHeight { get; set; }
        public bool Updatable { get; set; }
    }
}