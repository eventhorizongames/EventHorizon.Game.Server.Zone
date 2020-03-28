namespace EventHorizon.Zone.System.ClientEntities.Model
{
    using global::System.Collections.Generic;
    using EventHorizon.Zone.Core.Model.Core;

    public struct ClientEntityDetails
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public TransformState Transform { get; set; }
        public IList<string> TagList { get; set; }
        public Dictionary<string, object> Data { get; set; }
    }
}