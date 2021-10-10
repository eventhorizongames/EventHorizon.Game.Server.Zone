namespace EventHorizon.Zone.Core.Json
{
    using EventHorizon.Zone.Core.Api;

    using Newtonsoft.Json;

    public class NewtonsoftSerializeToJsonService
        : SerializeToJsonService
    {
        private static readonly JsonSerializerSettings SETTINGS = new()
        {
            Formatting = Formatting.Indented,
        };

        public string Serialize(
            object objectToSerialize
        ) => JsonConvert.SerializeObject(
            objectToSerialize,
            SETTINGS
        );
    }
}
