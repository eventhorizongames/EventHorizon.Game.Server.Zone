using System.Collections.Generic;
using EventHorizon.Zone.System.Server.Scripts.Model;
using MediatR;

namespace EventHorizon.Zone.System.Server.Scripts.Events.Run
{
    public struct RunServerScriptCommand : IRequest<ServerScriptResponse>
    {
        public string Id { get; }
        public IDictionary<string, object> Data { get; }

        public RunServerScriptCommand(
            string id,
            IDictionary<string, object> data
        )
        {
            Id = id;
            Data = data;
        }
    }
}