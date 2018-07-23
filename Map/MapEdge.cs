namespace EventHorizon.Game.Server.Zone.Map
{
    public struct MapEdge
    {
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