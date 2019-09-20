namespace EventHorizon.Zone.Core.Model.Admin
{
    public interface IAdminCommandResponse
    {
        string CommandFunction { get; }
        string RawCommand { get; }
        bool Success { get; }
        string Message { get; }
    }
}