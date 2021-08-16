using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventHorizon.Zone.Core.Model.Entity.State
{
    public interface EntityRepository
    {
        Task<List<IObjectEntity>> All();
        Task<IEnumerable<IObjectEntity>> Where(
            Func<IObjectEntity, bool> predicate
        );
        Task<IObjectEntity> FindById(
            long id
        );
        Task<IObjectEntity> Add(
            IObjectEntity entity
        );
        Task Update(
            EntityAction action,
            IObjectEntity entity
        );
        Task Remove(
            long id
        );
    }
}
