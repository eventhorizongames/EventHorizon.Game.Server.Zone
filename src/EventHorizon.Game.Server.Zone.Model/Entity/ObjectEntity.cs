using System.Collections.Generic;
using EventHorizon.Game.Server.Zone.Model.Core;

namespace EventHorizon.Game.Server.Zone.Model.Entity
{
    public interface IObjectEntity
    {
        long Id { get; set; }
        EntityType Type { get; }
        PositionState Position { get; set; }
        IList<string> TagList { get; set; }
        dynamic Data { get; set; }
        T GetProperty<T>(string prop);

        bool IsFound();
    }
}