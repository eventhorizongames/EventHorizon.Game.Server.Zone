namespace EventHorizon.Zone.System.Wizard.Model.Scripts;

using EventHorizon.Zone.System.Server.Scripts.Model;

using global::System.Collections.Generic;

public struct WizardServerScriptResponse
    : ServerScriptResponse
{
    public bool Success { get; }
    public string Message { get; }
    public IDictionary<string, string> Data { get; }

    public WizardServerScriptResponse(
        bool success,
        string message
    ) : this(
        success,
        message,
        new Dictionary<string, string>()
    )
    {
    }

    public WizardServerScriptResponse(
        bool success,
        string message,
        IDictionary<string, string> data
    )
    {
        Success = success;
        Message = message;
        Data = data;
    }
}
