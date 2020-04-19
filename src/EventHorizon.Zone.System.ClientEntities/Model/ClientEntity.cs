namespace EventHorizon.Zone.System.ClientEntities.Model
{
    using global::System.Collections.Generic;
    using EventHorizon.Zone.Core.Model.Core;
    using EventHorizon.Zone.Core.Model.Entity;

    public struct ClientEntity : IObjectEntity
    {
        public string ClientEntityId { get; }

        public string GlobalId => this.ClientEntityId;
        public EntityType Type => EntityType.OTHER;

        public long Id { get; set; }
        public string Name { get; set; }
        public TransformState Transform { get; set; }
        public IList<string> TagList { get; set; }
        public Dictionary<string, object> Data { get; }
        public Dictionary<string, object> RawData { get; set; }

        public ClientEntity(
            string clientEntityId,
            Dictionary<string, object> rawData
        )
        {
            ClientEntityId = clientEntityId;
            Id = -1;
            Name = string.Empty;
            Transform = default(TransformState);
            TagList = new List<string>();
            Data = new Dictionary<string, object>();
            RawData = rawData;
        }

        public bool IsFound() => !string.IsNullOrEmpty(
            ClientEntityId
        );
    }
}