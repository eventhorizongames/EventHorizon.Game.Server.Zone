using System.Collections.Generic;

namespace EventHorizon.Zone.System.Admin.Plugin.Command.Model.Standard
{
    public struct StandardAdminCommand : IAdminCommand
    {
        public string RawCommand { get; }
        public string Command { get; }
        public IList<string> Parts { get; }

        public StandardAdminCommand(
            string rawCommand,
            string command,
            IList<string> parts
        )
        {
            RawCommand = rawCommand;
            Command = command;
            Parts = parts;
        }
    }
}