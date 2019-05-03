using System.Collections.Generic;

namespace EventHorizon.Game.Server.Zone.Model.Admin
{
    public interface IAdminCommand
    {
        string RawCommand { get; }
        string Command { get; }
        IList<string> Parts { get; }
    }
}