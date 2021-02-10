namespace EventHorizon.Zone.System.Server.Scripts.Plugin.Compiler.Api
{
    using EventHorizon.Zone.System.Server.Scripts.Model.Details;
    using EventHorizon.Zone.System.Server.Scripts.Plugin.Compiler.Model;
    using global::System.Collections.Generic;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    public interface ServerScriptCompiler
    {
        /// <summary>
        /// Compile the Scripts into a single Packaged Library.
        /// </summary>
        /// <param name="scripts">The list of scripts to create a Packaged Library for.</param>
        /// <returns>Success/Failure and generated Hash.</returns>
        Task<CompiledScriptResult> Compile(
            IEnumerable<ServerScriptDetails> scripts,
            CancellationToken cancellationToken
        );
    }
}
