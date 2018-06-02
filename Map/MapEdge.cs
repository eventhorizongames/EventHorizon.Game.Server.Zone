namespace EventHorizon.Game.Server.Zone.Map
{
    public class MapEdge
    {
        public int FromIndex { get; set; }
        public int ToIndex { get; set; }
        public int Cost { get; set; } = 0;
    }
}