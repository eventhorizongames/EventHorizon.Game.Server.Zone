namespace EventHorizon.Game.Server.Zone.Model.Map
{
    public struct MapEdge
    {
        // TODO: Add test to make sure Path finding still works with default(MapEdge);
        public static MapEdge NULL = new MapEdge(-1, -1);
        
        public int FromIndex { get; set; }
        public int ToIndex { get; set; }
        public int Cost { get; set; }

        public MapEdge(int fromIndex, int toIndex)
        {
            this.FromIndex = fromIndex;
            this.ToIndex = toIndex;
            this.Cost = 0;
        }
    }
}