namespace EventHorizon.Zone.Core.Model.Entity;

using System.Collections.Generic;

public interface ObjectEntityData
    : IDictionary<string, object>
{
    IEnumerable<string> ForceSet { get; }
}
