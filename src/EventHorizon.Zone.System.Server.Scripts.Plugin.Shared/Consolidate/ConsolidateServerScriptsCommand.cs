namespace EventHorizon.Zone.System.Server.Scripts.Plugin.Shared.Consolidate
{
    using EventHorizon.Zone.Core.Model.Command;
    using EventHorizon.Zone.System.Server.Scripts.Model.Details;
    using EventHorizon.Zone.System.Server.Scripts.Plugin.Shared.Model;

    using global::System.Collections.Generic;

    using MediatR;

    public struct ConsolidateServerScriptsCommand
        : IRequest<CommandResult<ConsolidateServerScriptsResult>>
    {
        public IEnumerable<ServerScriptDetails> Scripts { get; }

        public ConsolidateServerScriptsCommand(
            IEnumerable<ServerScriptDetails> scripts
        )
        {
            Scripts = scripts;
        }
    }
}
