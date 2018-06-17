using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Core.IdPool;
using EventHorizon.Game.Server.Zone.Entity.Model;

namespace EventHorizon.Game.Server.Zone.Entity.State.Impl
{
    public class EntityRepository : IEntityRepository
    {
        private static readonly ConcurrentDictionary<long, IObjectEntity> ENTITIES = new ConcurrentDictionary<long, IObjectEntity>();

        readonly IIdPool _idPool;
        public EntityRepository(IIdPool idPool)
        {
            _idPool = idPool;
        }

        public Task<List<IObjectEntity>> All()
        {
            return Task.FromResult(ENTITIES.Values.ToList());
        }

        public Task<IObjectEntity> FindById(long id)
        {
            return Task.FromResult(
                ENTITIES.FirstOrDefault(a => a.Key == id).Value
            );
        }

        public Task<IObjectEntity> Add(IObjectEntity entity)
        {
            entity.Id = _idPool.NextId();
            ENTITIES.TryAdd(entity.Id, entity);
            return Task.FromResult(entity);
        }
        public Task Update(IObjectEntity entity)
        {
            ENTITIES.AddOrUpdate(entity.Id, entity, (key, oldEntity) => entity);

            return Task.CompletedTask;
        }
        public Task Remove(long id)
        {
            var entity = default(IObjectEntity);
            ENTITIES.TryRemove(id, out entity);
            return Task.CompletedTask;
        }
    }
}