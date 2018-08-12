namespace EventHorizon.Game.Server.Zone.Editor.Model
{
    public struct EditorAsset
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public dynamic Data { get; set; }
    }
}