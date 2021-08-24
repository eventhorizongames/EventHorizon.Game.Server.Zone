namespace EventHorizon.Zone.System.Player.Model.Details
{
    using EventHorizon.Zone.Core.Model.Core;

    using global::System.Collections.Concurrent;

    public struct PlayerDetails
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Locale { get; set; }
        public TransformState Transform { get; set; }
        public LocationState Location { get; set; }
        public ConcurrentDictionary<string, object> Data { get; set; }

        public bool IsNew()
        {
            return Id == null;
        }
    }
}
