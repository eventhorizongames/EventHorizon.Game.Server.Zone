namespace EventHorizon.Zone.System.Server.Scripts.Run.Model
{
    using EventHorizon.Zone.System.Server.Scripts.Model;
    using global::System.Collections.Generic;

    public class StandardServerScriptData
        : ServerScriptData
    {
        private readonly IDictionary<string, object> _data;

        public StandardServerScriptData(
            IDictionary<string, object> data
        )
        {
            _data = data;
        }

        public T Get<T>(
            string key
        )
        {
            return _data.GetValueOrDefault(
                key,
                default(T)
            );
        }
    }
}