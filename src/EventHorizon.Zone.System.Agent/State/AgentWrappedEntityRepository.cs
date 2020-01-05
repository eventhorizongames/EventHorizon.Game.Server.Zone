using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Agent.Model;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Agent.Model.State;
using EventHorizon.Zone.Core.Model.Entity.State;
using System;

namespace EventHorizon.Zone.System.Agent.State
{
    public class AgentWrappedEntityRepository : IAgentRepository
    {
        readonly EntityRepository _entityRepository;

        public AgentWrappedEntityRepository(
            EntityRepository entityRepository
        )
        {
            _entityRepository = entityRepository;
        }

        public async Task<IEnumerable<AgentEntity>> All()
        {
            return (await _entityRepository.All())
                .Where(
                    a => a.Type == EntityType.AGENT
                ).Cast<AgentEntity>();
        }

        public async Task<AgentEntity> FindById(long id)
        {
            return (await _entityRepository.All())
                .Where(
                    a => a.Id == id && a.Type == EntityType.AGENT
                ).Cast<AgentEntity>()
                .FirstOrDefault();
        }

        public async Task<AgentEntity> FindByAgentId(
            string agentId
        )
        {
            return (await this.All())
                .FirstOrDefault(
                    a => a.AgentId == agentId
                );
        }

        public Task Update(
            EntityAction action,
            AgentEntity agent
        )
        {
            return _entityRepository.Update(
                action,
                agent
            );
        }

        public async Task<IEnumerable<AgentEntity>> Where(
            Func<AgentEntity, bool> query
        )
        {
            return (await All()).Where(
                query
            );
        }
    }
}