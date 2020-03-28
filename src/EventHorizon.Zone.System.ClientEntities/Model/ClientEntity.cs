namespace EventHorizon.Zone.System.ClientEntities.Model
{
    using global::System.Collections.Generic;
    using EventHorizon.Zone.Core.Model.Core;
    using EventHorizon.Zone.Core.Model.Entity;

    public class ClientEntity : IObjectEntity
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
            RawData = rawData;
            TagList = new List<string>();
            Data = new Dictionary<string, object>();
        }

        public bool IsFound() => !string.IsNullOrEmpty(
            ClientEntityId
        );
    }
}