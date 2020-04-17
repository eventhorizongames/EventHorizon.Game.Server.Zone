using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.Entity.Action;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.Core.Model.Entity.State;
using EventHorizon.Zone.Core.Model.Id;
using MediatR;

namespace EventHorizon.Zone.Core.Entity.State
{
    public class InMemoryEntityRepository : EntityRepository
    {
        private readonly static ConcurrentDictionary<long, IObjectEntity> _entityMap = new ConcurrentDictionary<long, IObjectEntity>();

        readonly IMediator _mediator;
        readonly IdPool _idPool;

        public InMemoryEntityRepository(
            IMediator mediator,
            IdPool idPool
        )
        {
            _mediator = mediator;
            _idPool = idPool;
        }

        public Task<List<IObjectEntity>> All()
        {
            return Task.FromResult(
                _entityMap.Values.ToList()
            );
        }

        public Task<IEnumerable<IObjectEntity>> Where(
            Func<IObjectEntity, bool> predicate
        )
        {
            return Task.FromResult(
                _entityMap.Values.Where<IObjectEntity>(
                    predicate
                )
            );
        }

        public Task<IObjectEntity> FindById(
            long id
        )
        {
            return Task.FromResult(
                _entityMap.FirstOrDefault(
                    entity => entity.Key == id
                ).Value ?? DefaultEntity.NULL
            );
        }

        public Task<IObjectEntity> Add(
            IObjectEntity entity
        )
        {
            entity.Id = _idPool.NextId();
            _entityMap.TryAdd(
                entity.Id,
                entity
            );
            _mediator.Publish(
                new EntityActionEvent
                {
                    Action = EntityAction.ADD,
                    Entity = entity
                }
            );
            return Task.FromResult(
                entity
            );
        }
        public Task Update(
            EntityAction action,
            IObjectEntity entity
        )
        {
            _entityMap.AddOrUpdate(
                entity.Id,
                entity,
                (key, oldEntity) => entity
            );
            _mediator.Publish(
                new EntityActionEvent
                {
                    Action = action,
                    Entity = entity
                }
            );
            return Task.CompletedTask;
        }
        public Task Remove(
            long id
        )
        {
            var entity = default(IObjectEntity);
            _entityMap.TryRemove(
                id,
                out entity
            );
            _mediator.Publish(
                new EntityActionEvent
                {
                    Action = EntityAction.REMOVE,
                    Entity = entity
                }
            );
            return Task.CompletedTask;
        }
    }
}