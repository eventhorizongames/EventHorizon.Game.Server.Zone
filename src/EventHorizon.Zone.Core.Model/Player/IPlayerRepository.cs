namespace EventHorizon.Zone.Core.Model.Player;

using System.Threading.Tasks;

public interface IPlayerRepository
{
    Task<PlayerEntity> FindById(string id);
}
