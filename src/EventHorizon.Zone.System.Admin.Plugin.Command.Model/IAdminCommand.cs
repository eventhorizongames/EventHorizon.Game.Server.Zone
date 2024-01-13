namespace EventHorizon.Zone.System.Admin.Plugin.Command.Model;

using global::System.Collections.Generic;

public interface IAdminCommand
{
    string RawCommand { get; }
    string Command { get; }
    IList<string> Parts { get; }
}
