namespace EventHorizon.Zone.System.Wizard.Model.Scripts
{
    using EventHorizon.Zone.System.Server.Scripts.Model;

    public struct WizardServerScriptResponse
        : ServerScriptResponse
    {
        public bool Success { get; }
        public string Message { get; }

        public WizardServerScriptResponse(
            bool success,
            string message
        )
        {
            Success = success;
            Message = message;
        }
    }
}
