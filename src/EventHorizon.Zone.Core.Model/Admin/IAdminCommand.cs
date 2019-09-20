using System.Collections.Generic;

namespace EventHorizon.Zone.Core.Model.Admin
{
    public interface IAdminCommand
    {
        string RawCommand { get; }
        string Command { get; }
        IList<string> Parts { get; }
    }
}