namespace EventHorizon.Zone.System.Client.Scripts.Plugin.Compiler.Api
{
    using EventHorizon.Zone.System.Client.Scripts.Model;
    using EventHorizon.Zone.System.Client.Scripts.Plugin.Compiler.Model;
    using global::System.Collections.Generic;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    public interface ClientScriptCompiler
    {
        /// <summary>
        /// Compile the Scripts into a single Base64 encoded string.
        /// </summary>
        /// <param name="scripts">The list of strings to create an artifact for.</param>
        /// <returns>Consolidated Base64 encoded string of scripts.</returns>
        Task<CompiledScriptResult> Compile(
            IEnumerable<ClientScript> scripts,
            CancellationToken cancellationToken
        );
    }
}
