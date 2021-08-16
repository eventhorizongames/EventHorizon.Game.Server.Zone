namespace EventHorizon.Zone.System.Client.Scripts.Plugin.Shared.Consolidate
{
    using EventHorizon.Zone.Core.Model.Command;
    using EventHorizon.Zone.System.Client.Scripts.Model;
    using EventHorizon.Zone.System.Client.Scripts.Plugin.Shared.Model;

    using global::System.Collections.Generic;

    using MediatR;

    public struct ConsolidateClientScriptsCommand
        : IRequest<CommandResult<ConsolidateClientScriptsResult>>
    {
        public IEnumerable<ClientScript> Scripts { get; }

        public ConsolidateClientScriptsCommand(
            IEnumerable<ClientScript> scripts
        )
        {
            Scripts = scripts;
        }
    }
}
