namespace EventHorizon.Zone.System.Server.Scripts.Model
{
    public class StandardServerScriptResponse
        : ServerScriptResponse
    {
        public bool Success { get; }
        public string Message { get; }

        public StandardServerScriptResponse(
            bool success,
            string message
        )
        {
            Success = success;
            Message = message;
        }
    }
}
