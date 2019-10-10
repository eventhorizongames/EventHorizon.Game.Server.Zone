using System.Collections.Generic;

namespace EventHorizon.Zone.System.Admin.Plugin.Command.Model
{
    public interface IAdminCommand
    {
        string RawCommand { get; }
        string Command { get; }
        IList<string> Parts { get; }
    }
}