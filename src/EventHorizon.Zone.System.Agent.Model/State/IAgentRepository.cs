using System.Collections.Generic;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Model.Entity;

namespace EventHorizon.Zone.System.Agent.Model.State
{
    public interface IAgentRepository
    {
        Task<IEnumerable<AgentEntity>> All();
        Task<AgentEntity> FindById(long entityId);
        Task<AgentEntity> FindByAgentId(string agentId);
        Task Update(EntityAction action, AgentEntity entity);
    }
}