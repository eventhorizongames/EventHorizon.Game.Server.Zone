using System.Collections.Generic;
using EventHorizon.Game.Server.Zone.Core.Model;

namespace EventHorizon.Game.Server.Zone.Entity.Model
{
    public struct DefaultEntity : IObjectEntity
    {
        public long Id { get; set; }

        public EntityType Type { get { return EntityType.OTHER; } }

        public PositionState Position { get; set; }
        public IList<string> TagList { get; set; }
        public dynamic Data { get; set; }

        public bool IsFound()
        {
            return false;
        }
    }
}