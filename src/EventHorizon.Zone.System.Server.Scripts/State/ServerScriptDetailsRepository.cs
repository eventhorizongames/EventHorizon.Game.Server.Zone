using System;
using System.Collections.Generic;
using EventHorizon.Zone.System.Server.Scripts.Model.Details;

namespace EventHorizon.Zone.System.Server.Scripts.State
{
    public interface ServerScriptDetailsRepository
    {
        void Add(
            string id,
            ServerScriptDetails script
        );
        IEnumerable<ServerScriptDetails> Where(
            Func<ServerScriptDetails, bool> query
        );
    }
}