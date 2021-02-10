namespace EventHorizon.Zone.System.Server.Scripts.Model
{
    public interface ServerScriptData
    {
        T Get<T>(string key);
    }
}
