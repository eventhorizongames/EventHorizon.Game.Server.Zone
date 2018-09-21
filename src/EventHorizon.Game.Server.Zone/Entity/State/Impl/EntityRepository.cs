using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Core.IdPool;
using EventHorizon.Game.Server.Zone.Entity.Action;
using EventHorizon.Game.Server.Zone.Entity.Model;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Entity.State.Impl
{
    public class EntityRepository : IEntityRepository
    {
        private static readonly ConcurrentDictionary<long, IObjectEntity> ENTITIES = new ConcurrentDictionary<long, IObjectEntity>();

        readonly IMediator _mediator;
        readonly IIdPool _idPool;
        public EntityRepository(IMediator mediator, IIdPool idPool)
        {
            _mediator = mediator;
            _idPool = idPool;
        }

        public Task<List<IObjectEntity>> All()
        {
            return Task.FromResult(ENTITIES.Values.ToList());
        }

        public Task<IObjectEntity> FindById(long id)
        {
            return Task.FromResult(
                ENTITIES.FirstOrDefault(a => a.Key == id).Value ?? default(DefaultEntity)
            );
        }

        public Task<IObjectEntity> Add(IObjectEntity entity)
        {
            entity.Id = _idPool.NextId();
            ENTITIES.TryAdd(entity.Id, entity);
            _mediator.Publish(new EntityActionEvent
            {
                Action = EntityAction.ADD,
                Entity = entity
            });
            return Task.FromResult(entity);
        }
        public Task Update(EntityAction action, IObjectEntity entity)
        {
            ENTITIES.AddOrUpdate(entity.Id, entity, (key, oldEntity) => entity);
            _mediator.Publish(new EntityActionEvent
            {
                Action = action,
                Entity = entity
            });
            return Task.CompletedTask;
        }
        public Task Remove(long id)
        {
            var entity = default(IObjectEntity);
            ENTITIES.TryRemove(id, out entity);
            _mediator.Publish(new EntityActionEvent
            {
                Action = EntityAction.REMOVE,
                Entity = entity
            });
            return Task.CompletedTask;
        }
    }
}