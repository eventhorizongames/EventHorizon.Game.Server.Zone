namespace EventHorizon.Zone.System.ClientEntities.Model
{
    using EventHorizon.Zone.Core.Model.Core;
    using global::System.Collections.Concurrent;
    using global::System.Collections.Generic;

    public struct ClientEntityDetails
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public TransformState Transform { get; set; }
        public IList<string> TagList { get; set; }
        public ConcurrentDictionary<string, object> Data { get; set; }
    }
}