namespace EventHorizon.Zone.System.Server.Scripts.State
{
    using EventHorizon.Zone.System.Server.Scripts.Model.Details;

    using global::System;
    using global::System.Collections.Generic;

    public interface ServerScriptDetailsRepository
    {
        IEnumerable<ServerScriptDetails> All { get; }

        ServerScriptDetails Find(
            string id
        );
        void Clear();

        void Add(
            string id,
            ServerScriptDetails script
        );

        IEnumerable<ServerScriptDetails> Where(
            Func<ServerScriptDetails, bool> query
        );
    }
}
