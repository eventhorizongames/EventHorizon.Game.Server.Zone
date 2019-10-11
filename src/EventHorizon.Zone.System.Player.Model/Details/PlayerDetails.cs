using System.Collections.Generic;
using EventHorizon.Zone.System.Player.Model.Position;

namespace EventHorizon.Zone.System.Player.Model.Details
{
    public struct PlayerDetails
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Locale { get; set; }
        public PlayerPositionState Position { get; set; }
        public Dictionary<string, object> Data { get; set; }

        public bool IsNew()
        {
            return this.Id == null;
        }
    }
}