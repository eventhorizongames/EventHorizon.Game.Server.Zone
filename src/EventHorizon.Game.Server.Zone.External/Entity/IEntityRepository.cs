using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Entity.Model;
using EventHorizon.Game.Server.Zone.Model.Entity;

namespace EventHorizon.Game.Server.Zone.External.Entity
{
    public interface IEntityRepository
    {
        Task<List<IObjectEntity>> All();
        Task<IEnumerable<IObjectEntity>> Where(
            Func<IObjectEntity, bool> predicate
        );
        Task<IObjectEntity> FindById(long id);
        Task<IObjectEntity> Add(IObjectEntity entity);
        Task Update(EntityAction action, IObjectEntity entity);
        Task Remove(long id);
    }
}