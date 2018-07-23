using System.Linq;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Agent.Model;
using EventHorizon.Game.Server.Zone.Entity.Model;
using EventHorizon.Game.Server.Zone.Entity.State;

namespace EventHorizon.Game.Server.Zone.Agent.Move.Repository.Impl
{
    public class AgentRepository : IAgentRepository
    {
        readonly IEntityRepository _entityRepository;

        public AgentRepository(IEntityRepository entityRepository)
        {
            _entityRepository = entityRepository;
        }

        public async Task<AgentEntity> FindById(long id)
        {
            var entityList = await _entityRepository.All();
            return entityList
                .FindAll(a => a.Type == EntityType.AGENT)
                .Cast<AgentEntity>()
                .FirstOrDefault(a => a.Id == id);
        }

        public Task Update(AgentEntity agent)
        {
            return _entityRepository.Update(agent);
        }
    }
}