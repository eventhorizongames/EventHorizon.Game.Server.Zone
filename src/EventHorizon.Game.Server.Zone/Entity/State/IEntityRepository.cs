using System.Collections.Generic;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Entity.Model;
using EventHorizon.Game.Server.Zone.Model.Entity;

namespace EventHorizon.Game.Server.Zone.Entity.State
{
    public interface IEntityRepository
    {
        Task<List<IObjectEntity>> All();
        Task<IObjectEntity> FindById(long id);
        Task<IObjectEntity> Add(IObjectEntity entity);
        Task Update(EntityAction action, IObjectEntity entity);
        Task Remove(long id);
    }
}