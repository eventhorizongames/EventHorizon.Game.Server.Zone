namespace EventHorizon.Zone.Core.Entity.State
{
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

    public class InMemoryEntityRepository : EntityRepository
    {
        // TODO: Update this to not use static.
        // Move the IMediator publish events out of this and into the events.
        // Also remove the Mutation methods from entityRepository, 
        // Move mutation abstraction into a separate Mutation internal abstraction.
        private readonly static ConcurrentDictionary<long, IObjectEntity> _entityMap = new ConcurrentDictionary<long, IObjectEntity>();

        private readonly IMediator _mediator;
        private readonly IdPool _idPool;

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
            return _entityMap.Values.ToList().FromResult();
        }

        public Task<IEnumerable<IObjectEntity>> Where(
            Func<IObjectEntity, bool> predicate
        )
        {
            return _entityMap.Values.Where(
                predicate
            ).FromResult();
        }

        public Task<IObjectEntity> FindById(
            long id
        )
        {
            return (_entityMap.FirstOrDefault(
                entity => entity.Key == id
            ).Value ?? DefaultEntity.NULL).FromResult();
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
            return entity.FromResult();
        }

        public Task Update(
            EntityAction action,
            IObjectEntity entity
        )
        {
            _entityMap.AddOrUpdate(
                entity.Id,
                entity,
                (_, __) => entity
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
            _entityMap.TryRemove(
                id,
                out var entity
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