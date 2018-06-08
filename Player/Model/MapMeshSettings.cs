namespace EventHorizon.Game.Server.Zone.Player.Model
{
    public class MapMeshSettings
    {
        public string Name { get; set; }
        public string HeightMapUrl { get; set; }
        public bool IsPickable { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Subdivisions { get; set; }
        public int MinHeight { get; set; }
        public int MaxHeight { get; set; }
        public bool Updatable { get; set; }
    }
}