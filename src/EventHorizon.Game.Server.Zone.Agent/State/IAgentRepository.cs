using System.Collections.Generic;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Agent.Model;
using EventHorizon.Game.Server.Zone.Entity.Model;
using EventHorizon.Game.Server.Zone.Model.Entity;

namespace EventHorizon.Game.Server.Zone.State.Repository
{
    public interface IAgentRepository
    {
        Task<IEnumerable<AgentEntity>> All();
        Task<AgentEntity> FindById(long entityId);
        Task<AgentEntity> FindByAgentId(string agentId);
        Task Update(EntityAction action, AgentEntity entity);
    }
}