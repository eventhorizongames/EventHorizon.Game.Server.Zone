using System.Collections.Generic;
using EventHorizon.Game.Server.Zone.Model.Core;

namespace EventHorizon.Game.Server.Zone.Model.Entity
{
    public interface IObjectEntity
    {
        long Id { get; set; }
        string Name { get; set; }
        EntityType Type { get; }
        PositionState Position { get; set; }
        IList<string> TagList { get; set; }
        Dictionary<string, object> Data { get; }
        Dictionary<string, object> RawData { get; set; }

        bool IsFound();
    }
}