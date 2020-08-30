namespace EventHorizon.Zone.System.Client.Scripts.Plugin.Compiler.Api
{
    using EventHorizon.Zone.System.Client.Scripts.Model;
    using global::System.Collections.Generic;

    public interface ClientScriptsConsolidator
    {
        string IntoSingleTemplatedString(
            IEnumerable<ClientScript> scripts,
            ref List<string> usingList
        );
    }
}
