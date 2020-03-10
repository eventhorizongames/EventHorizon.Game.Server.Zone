using System.Threading.Tasks;

namespace EventHorizon.Zone.Core.Model.Player
{
    public interface IPlayerRepository
    {
        Task<PlayerEntity> FindById(string id);
    }
}