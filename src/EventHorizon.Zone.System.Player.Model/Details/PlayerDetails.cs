namespace EventHorizon.Zone.System.Player.Model.Details
{
    using global::System.Collections.Generic;
    using EventHorizon.Zone.Core.Model.Core;

    public struct PlayerDetails
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Locale { get; set; }
        public TransformState Transform { get; set; }
        public LocationState Location { get; set; }
        public Dictionary<string, object> Data { get; set; }

        public bool IsNew()
        {
            return this.Id == null;
        }
    }
}