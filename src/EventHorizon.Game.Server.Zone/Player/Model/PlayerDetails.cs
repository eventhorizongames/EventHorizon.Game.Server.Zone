using System.Collections;
using System.Collections.Generic;

namespace EventHorizon.Game.Server.Core.Player.Model
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