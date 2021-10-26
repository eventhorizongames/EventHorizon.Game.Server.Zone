namespace EventHorizon.Zone.System.Player.Tests.TestingModels
{
    using EventHorizon.Zone.Core.Model.Entity;

    using global::System.Collections.Generic;

    public class ObjectEntityDataTestModel
        : Dictionary<string, object>,
        ObjectEntityData
    {
        public IEnumerable<string> ForceSet { get; set; } = new List<string>();
    }
}
