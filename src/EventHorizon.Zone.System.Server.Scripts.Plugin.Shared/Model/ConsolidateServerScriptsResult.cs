namespace EventHorizon.Zone.System.Server.Scripts.Plugin.Shared.Model
{
    using global::System.Collections.Generic;

    public struct ConsolidateServerScriptsResult
    {
        public IEnumerable<string> UsingList { get; }
        public string ScriptClasses { get; }
        public string ConsolidatedScripts { get; }

        public ConsolidateServerScriptsResult(
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
}
