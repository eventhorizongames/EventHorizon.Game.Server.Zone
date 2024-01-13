namespace EventHorizon.Zone.System.Agent.Model.State;

using global::System;
using global::System.Collections.Generic;
using global::System.Threading.Tasks;

using EventHorizon.Zone.Core.Model.Entity;

public interface IAgentRepository
{
    Task<IEnumerable<AgentEntity>> All();
    Task<IEnumerable<AgentEntity>> Where(
        Func<AgentEntity, bool> query
    );
    Task<AgentEntity> FindById(
        long entityId
    );
    Task<AgentEntity> FindByAgentId(
        string agentId
    );
    Task Update(
        EntityAction action,
        AgentEntity entity
    );
}
