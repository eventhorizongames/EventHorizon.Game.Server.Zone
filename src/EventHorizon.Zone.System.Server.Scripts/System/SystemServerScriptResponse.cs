using EventHorizon.Zone.System.Server.Scripts.Model;

namespace EventHorizon.Zone.System.Server.Scripts.System
{
    public struct SystemServerScriptResponse : ServerScriptResponse
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