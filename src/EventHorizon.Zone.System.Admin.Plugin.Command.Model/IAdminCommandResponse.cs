namespace EventHorizon.Zone.System.Admin.Plugin.Command.Model
{
    public interface IAdminCommandResponse
    {
        string CommandFunction { get; }
        string RawCommand { get; }
        bool Success { get; }
        string Message { get; }
    }
}