namespace EventHorizon.Zone.System.Client.Scripts.Plugin.Shared.Consolidate;

using EventHorizon.Zone.Core.Model.Command;
using EventHorizon.Zone.System.Client.Scripts.Plugin.Shared.Model;

using global::System;
using global::System.Collections.Generic;
using global::System.IO;
using global::System.Linq;
using global::System.Text;
using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

public class ConsolidatClientScriptsCommandHandler
    : IRequestHandler<ConsolidateClientScriptsCommand, CommandResult<ConsolidateClientScriptsResult>>
{
    private readonly static string AssemblyScriptTemplate = @"                        
[[USING_SECTION]]

[[SCRIPT_CLASSES]]
";

    public Task<CommandResult<ConsolidateClientScriptsResult>> Handle(
        ConsolidateClientScriptsCommand request,
        CancellationToken cancellationToken
    )
    {
        var scripts = request.Scripts.OrderBy(a => a.Name);
        var stringBuilder = new StringBuilder(
            string.Empty
        );
        var usingList = new List<string>();

        foreach (var script in scripts)
        {
            var name = script.Name;
            var (scriptUsingList, scriptContent) = SplitScriptContent(
                script.ScriptString
            );
            foreach (var scriptUsing in scriptUsingList)
            {
                if (!usingList.Contains(
                    scriptUsing
                ))
                {
                    usingList.Add(
                        scriptUsing
                    );
                }
            }

            stringBuilder.AppendLine(
                scriptContent.Replace(
                    "__SCRIPT__",
                    name.Replace(".csx", string.Empty)
                )
            );
        }

        var scriptClasses = stringBuilder.ToString();
        var consolidatedScripts = AssemblyScriptTemplate.Replace(
            "[[USING_SECTION]]",
            string.Join(
                Environment.NewLine,
                usingList.OrderBy(a => a)
            )
        ).Replace(
            "[[SCRIPT_CLASSES]]",
            scriptClasses
        );

        return new CommandResult<ConsolidateClientScriptsResult>(
            new ConsolidateClientScriptsResult(
                usingList,
                scriptClasses,
                consolidatedScripts
            )
        ).FromResult();
    }

    private static (IList<string>, string) SplitScriptContent(
        string scriptString
    )
    {
        var scriptContent = string.Empty;
        var usingList = new List<string>();
        using var reader = new StringReader(
            scriptString
        );
        var line = default(string);
        while ((line = reader.ReadLine()) != null)
        {
            // Extract the any using from script
            if (line.StartsWith("using "))
            {
                usingList.Add(
                    line
                );
                continue;
            }
            // Append any other content to the scriptContent
            scriptContent += line;
            scriptContent += Environment.NewLine;
        }
        return (usingList, scriptContent);
    }
}
