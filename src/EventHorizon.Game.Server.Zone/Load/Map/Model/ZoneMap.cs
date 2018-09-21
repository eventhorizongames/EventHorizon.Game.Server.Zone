using System.Collections.Generic;

namespace EventHorizon.Game.Server.Zone.Load.Map.Model
{
    public class ZoneMap
    {
        public int Dimensions { get; set; }
        public int TileDimensions { get; set; }
        public ZoneMapMesh Mesh { get; set; }
    }
    public class ZoneMapMesh
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