namespace EventHorizon.Game.Server.Zone.Admin.Command.Scripts.Model
{
    public struct AdminCommandScriptResponse
    {
        public bool Success { get; }
        public string Message { get; }
        public AdminCommandScriptResponse(
            bool success,
            string message
        )
        {
            Success = success;
            Message = message;
        }
    }
}