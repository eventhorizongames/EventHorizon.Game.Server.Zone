namespace EventHorizon.Zone.System.Server.Scripts.Model
{
    public interface ServerScriptResponse
    {
        bool Success { get; }
        string Message { get; }
    }
}