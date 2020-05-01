namespace EventHorizon.Zone.Core.Map.Model
{
    using EventHorizon.Zone.Core.Model.Map;

    public struct ZoneMapDetails : IMapDetails
    {
        public int Dimensions { get; set; }
        public int TileDimensions { get; set; }
        public ZoneMapMesh Mesh { get; set; }
    }
}
