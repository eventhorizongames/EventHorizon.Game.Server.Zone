namespace EventHorizon.Zone.Core.Entity.Tests.TestingModels;

using System.Collections.Generic;

using EventHorizon.Zone.Core.Model.Entity;

public class ObjectEntityDataTestModel
    : Dictionary<string, object>,
    ObjectEntityData
{
    public IEnumerable<string> ForceSet { get; set; } = new List<string>();
}
