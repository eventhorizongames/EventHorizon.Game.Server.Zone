namespace EventHorizon.Zone.System.Admin.Plugin.Command.Model.Builder;

using global::System;
using global::System.Collections.Generic;
using global::System.Linq;

using EventHorizon.Zone.System.Admin.Plugin.Command.Model.Standard;

public static class BuildAdminCommand
{
    public static IAdminCommand FromString(
        string rawCommand
    )
    {
        return Parse(
            rawCommand
        );
    }

    private static IAdminCommand Parse(
        string rawCommand
    )
    {
        var commandQuoteSplit = rawCommand.Split(
            new char[] { '"' },
            StringSplitOptions.RemoveEmptyEntries
        );
        if (commandQuoteSplit.Length > 1)
        {
            // Parse the command
            var command = ParseOutCommandAndParts(
                commandQuoteSplit[0]
            );
            for (int i = 1; i < commandQuoteSplit.Length; i++)
            {
                var commandItem = commandQuoteSplit[i];
                if (!string.IsNullOrWhiteSpace(
                    commandItem
                ))
                {
                    command.Parts.Add(
                        commandItem.Trim()
                    );
                }
            }
            return command;
        }
        return ParseOutCommandAndParts(
            rawCommand
        );
    }

    private static IAdminCommand ParseOutCommandAndParts(
        string rawCommand
    )
    {
        var commandAndPartsSplit = rawCommand.Split(
            new char[] { ' ' },
            StringSplitOptions.RemoveEmptyEntries
        );
        var commandFunction = commandAndPartsSplit[0];
        var parts = new List<string>();
        for (int i = 1; i < commandAndPartsSplit.Length; i++)
        {
            parts.Add(
                commandAndPartsSplit[i]
            );
        }
        return new StandardAdminCommand(
            rawCommand,
            commandFunction,
            parts
        );
    }
}
