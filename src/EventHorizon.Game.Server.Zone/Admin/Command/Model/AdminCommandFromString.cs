using System;
using System.Collections.Generic;
using System.Linq;
using EventHorizon.Game.Server.Zone.Model.Admin;

namespace EventHorizon.Game.Server.Zone.Admin.Command.Model
{
    public struct AdminCommandFromString : IAdminCommand
    {
        public string RawCommand { get; private set; }

        public string Command { get; private set; }

        public IList<string> Parts { get; private set; }

        public static IAdminCommand CreateFromString(string rawCommand)
        {
            return Parse(
                new AdminCommandFromString
                {
                    RawCommand = rawCommand
                },
                rawCommand
            );
        }

        private static AdminCommandFromString Parse(
            AdminCommandFromString command,
            string rawCommand
        )
        {
            var commandQuoteSplit = rawCommand.Split(new char[] { '"' }, StringSplitOptions.RemoveEmptyEntries);
            if (commandQuoteSplit.Length > 1)
            {
                // Parse the command
                command = ParseOutCommandAndParts(
                    command,
                    commandQuoteSplit[0]
                );
                command.Parts.Add(commandQuoteSplit.Last());
                return command;
            }
            return ParseOutCommandAndParts(
                command,
                rawCommand
            );
        }

        private static AdminCommandFromString ParseOutCommandAndParts(
            AdminCommandFromString command,
            string commandAndParts
        )
        {
            var commandAndPartsSplit = commandAndParts.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var commandFunction = commandAndPartsSplit[0];
            var parts = new List<string>();
            for (int i = 1; i < commandAndPartsSplit.Length; i++)
            {
                parts.Add(commandAndPartsSplit[i]);
            }
            command.Command = commandFunction;
            command.Parts = parts;
            return command;
        }
    }
}