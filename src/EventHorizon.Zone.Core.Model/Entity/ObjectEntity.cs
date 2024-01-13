namespace EventHorizon.Zone.Core.Model.Entity;

using System.Collections.Concurrent;
using System.Collections.Generic;

using EventHorizon.Zone.Core.Model.Core;

public interface IObjectEntity
{
    long Id { get; set; }
    string Name { get; set; }
    string GlobalId { get; }
    EntityType Type { get; }
    TransformState Transform { get; set; }
    IList<string>? TagList { get; set; }
    ConcurrentDictionary<string, object> Data { get; }
    ConcurrentDictionary<string, object> RawData { get; set; }

    bool IsFound();
}
