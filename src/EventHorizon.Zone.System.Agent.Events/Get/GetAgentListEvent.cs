namespace EventHorizon.Zone.System.Agent.Events.Get
{
    using global::System;
    using global::System.Collections.Generic;
    using EventHorizon.Zone.System.Agent.Model;
    using MediatR;

    public struct GetAgentListEvent : IRequest<IEnumerable<AgentEntity>>
    {
        public Func<AgentEntity, bool> Query { get; set; }

        public GetAgentListEvent(
            Func<AgentEntity, bool> query
        )
        {
            Query = query;
        }
    }
}