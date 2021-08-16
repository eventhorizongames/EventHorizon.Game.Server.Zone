namespace EventHorizon.Zone.System.Server.Scripts.System
{
    using EventHorizon.Zone.System.Server.Scripts.Model;

    public struct SystemServerScriptResponse
        : ServerScriptResponse
    {
        public bool Success { get; }
        public string Message { get; }

        public SystemServerScriptResponse(
            bool success,
            string message
        )
        {
            Success = success;
            Message = message;
        }
    }
}
