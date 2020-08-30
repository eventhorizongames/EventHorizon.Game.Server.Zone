namespace EventHorizon.Zone.System.Client.Scripts.Plugin.Compiler.Consolidate
{
    using EventHorizon.Zone.System.Client.Scripts.Model;
    using EventHorizon.Zone.System.Client.Scripts.Plugin.Compiler.Api;
    using global::System;
    using global::System.Collections.Generic;
    using global::System.IO;
    using global::System.Text;

    public class StandardClientScriptsConsolidator
        : ClientScriptsConsolidator
    {
        public string IntoSingleTemplatedString(
            IEnumerable<ClientScript> scripts,
            ref List<string> usingList
        )
        {
            var stringBuilder = new StringBuilder(
                string.Empty
            );

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

            return stringBuilder.ToString();
        }

        private (IList<string>, string) SplitScriptContent(
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
}
