using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Agent.Model;

namespace EventHorizon.Game.Server.Zone.State.Repository
{
    public interface IAgentRepository
    {
        Task<AgentEntity> FindById(long entityId);
        Task Update(AgentEntity entity);
    }
}