using System.Collections.Generic;
using EventHorizon.Zone.Core.Model.Core;

namespace EventHorizon.Zone.Core.Model.Entity
{
    public interface IObjectEntity
    {
        long Id { get; set; }
        string Name { get; set; }
        string GlobalId { get; }
        EntityType Type { get; }
        PositionState Position { get; set; }
        IList<string> TagList { get; set; }
        Dictionary<string, object> Data { get; }
        Dictionary<string, object> RawData { get; set; }

        bool IsFound();
    }
}