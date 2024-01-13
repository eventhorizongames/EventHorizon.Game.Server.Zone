namespace EventHorizon.Zone.System.Client.Scripts.Plugin.Shared.Model;

using global::System.Collections.Generic;

public struct ConsolidateClientScriptsResult
{
    public IEnumerable<string> UsingList { get; }
    public string ScriptClasses { get; }
    public string ConsolidatedScripts { get; }

    public ConsolidateClientScriptsResult(
        IEnumerable<string> usingList,
        string scriptClasses,
        string consolidatedScripts
    )
    {
        UsingList = usingList;
        ScriptClasses = scriptClasses;
        ConsolidatedScripts = consolidatedScripts;
    }
}
