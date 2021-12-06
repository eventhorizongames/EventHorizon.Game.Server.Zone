namespace EventHorizon.Zone.System.Agent.Events.Get;

using EventHorizon.Zone.System.Agent.Model;

using global::System;
using global::System.Collections.Generic;

using MediatR;

public record GetAgentListEvent(
    Func<AgentEntity, bool>? Query = null
) : IRequest<IEnumerable<AgentEntity>>;
