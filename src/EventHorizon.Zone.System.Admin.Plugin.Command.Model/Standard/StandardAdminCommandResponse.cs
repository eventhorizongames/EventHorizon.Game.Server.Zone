namespace EventHorizon.Zone.System.Admin.Plugin.Command.Model.Standard;

public struct StandardAdminCommandResponse : IAdminCommandResponse
{
    public string CommandFunction { get; }
    public string RawCommand { get; }
    public bool Success { get; }
    public string Message { get; }

    public StandardAdminCommandResponse(
        string commandFunction,
        string rawCommand,
        bool success,
        string message
    )
    {
        CommandFunction = commandFunction;
        RawCommand = rawCommand;
        Success = success;
        Message = message;
    }
}
