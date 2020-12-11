namespace EventHorizon.Zone.System.ClientEntities.Model
{
    using EventHorizon.Zone.Core.Model.Core;
    using EventHorizon.Zone.Core.Model.Entity;
    using global::System.Collections.Concurrent;
    using global::System.Collections.Generic;

    public struct ClientEntity : IObjectEntity
    {
        public string ClientEntityId => GlobalId;

        public string GlobalId { get; set; }
        public EntityType Type => EntityType.OTHER;

        public long Id { get; set; }
        public string Name { get; set; }
        public TransformState Transform { get; set; }
        public IList<string> TagList { get; set; }
        public ConcurrentDictionary<string, object> Data { get; set; }
        public ConcurrentDictionary<string, object> RawData { get; set; }

        public ClientEntity(
            string clientEntityId,
            ConcurrentDictionary<string, object> rawData
        )
        {
            Id = -1;
            GlobalId = clientEntityId;
            Name = string.Empty;
            Transform = default(TransformState);
            TagList = new List<string>();
            Data = new ConcurrentDictionary<string, object>();
            RawData = rawData;
        }

        public bool IsFound() => !string.IsNullOrEmpty(
            ClientEntityId
        );
    }
}