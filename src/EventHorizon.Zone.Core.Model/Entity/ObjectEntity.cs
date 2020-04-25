namespace EventHorizon.Zone.Core.Model.Entity
{
    using EventHorizon.Zone.Core.Model.Core;
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    public interface IObjectEntity
    {
        long Id { get; set; }
        string Name { get; set; }
        string GlobalId { get; }
        EntityType Type { get; }
        TransformState Transform { get; set; }
        IList<string> TagList { get; set; }
        ConcurrentDictionary<string, object> Data { get; }
        ConcurrentDictionary<string, object> RawData { get; set; }

        bool IsFound();
    }
}