namespace EventHorizon.Game.Server.Core.Player.Model
{
    public struct PlayerDetails
    {
        public string Id { get; set; }
        public PlayerPositionState Position { get; set; }
        public dynamic Data { get; set; }

        public bool IsNew()
        {
            return this.Id == null;
        }
    }
}