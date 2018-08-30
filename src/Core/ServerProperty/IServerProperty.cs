namespace EventHorizon.Game.Server.Zone.Core.ServerProperty
{
    public interface IServerProperty
    {
        T Get<T>(string key);
        void Set(string key, object value);
    }
}