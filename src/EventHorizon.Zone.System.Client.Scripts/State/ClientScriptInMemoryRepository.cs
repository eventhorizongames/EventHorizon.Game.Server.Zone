﻿namespace EventHorizon.Zone.System.Client.Scripts.State
{
    using EventHorizon.Zone.System.Client.Scripts.Api;
    using EventHorizon.Zone.System.Client.Scripts.Model;

    using global::System.Collections.Concurrent;
    using global::System.Collections.Generic;

    public class ClientScriptInMemoryRepository
        : ClientScriptRepository
    {
        private readonly ConcurrentDictionary<string, ClientScript> SCRIPT_MAP = new ConcurrentDictionary<string, ClientScript>();

        public void Add(
            ClientScript script
        )
        {
            SCRIPT_MAP.AddOrUpdate(
                script.Name,
                script,
                (_, __) => script
            );
        }

        public IEnumerable<ClientScript> All()
        {
            return SCRIPT_MAP.Values;
        }
    }
}
