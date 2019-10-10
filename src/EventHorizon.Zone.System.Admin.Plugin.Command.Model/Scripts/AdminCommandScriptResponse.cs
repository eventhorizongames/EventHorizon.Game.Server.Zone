using EventHorizon.Zone.System.Server.Scripts.Model;

namespace EventHorizon.Zone.System.Admin.Plugin.Command.Model.Scripts
{
    /// <summary>
    /// Used in Scripts to create Script Responses.
    /// </summary>
    public class AdminCommandScriptResponse : ServerScriptResponse
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