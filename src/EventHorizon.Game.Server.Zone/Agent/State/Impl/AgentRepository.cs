using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Agent.Model;
using EventHorizon.Game.Server.Zone.Entity.Model;
using EventHorizon.Game.Server.Zone.Entity.State;
using EventHorizon.Game.Server.Zone.External.Entity;
using EventHorizon.Game.Server.Zone.Model.Entity;
using EventHorizon.Game.Server.Zone.State.Repository;

namespace EventHorizon.Game.Server.Zone.Agent.State.Impl
{
    public class AgentRepository : IAgentRepository
    {
        readonly IEntityRepository _entityRepository;

        public AgentRepository(IEntityRepository entityRepository)
        {
            _entityRepository = entityRepository;
        }

        public async Task<IEnumerable<AgentEntity>> All()
        {
            return (await _entityRepository.All())
                .Where(a => a.Type == EntityType.AGENT)
                .Cast<AgentEntity>();
        }

        public async Task<AgentEntity> FindById(long id)
        {
            return (await _entityRepository.All())
                .Where(a => a.Id == id && a.Type == EntityType.AGENT)
                .Cast<AgentEntity>()
                .FirstOrDefault();
        }

        public async Task<AgentEntity> FindByAgentId(string agentId)
        {
            return (await this.All())
                .FirstOrDefault(a => a.AgentId == agentId);
        }

        public Task Update(EntityAction action, AgentEntity agent)
        {
            return _entityRepository.Update(action, agent);
        }
    }
}