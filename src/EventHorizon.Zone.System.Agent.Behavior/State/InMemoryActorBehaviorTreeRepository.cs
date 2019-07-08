using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using EventHorizon.Zone.System.Agent.Behavior.Api;
using EventHorizon.Zone.System.Agent.Behavior.Model;
using EventHorizon.Zone.System.Agent.Behavior.Script;

namespace EventHorizon.Zone.System.Agent.Behavior.State
{
    public class InMemoryActorBehaviorTreeRepository : ActorBehaviorTreeRepository
    {
        private static readonly ActorBehaviorTreeShape DEFAULT_TREE_SHAPE = new ActorBehaviorTreeShape
        {
            NodeList = new List<BehaviorNode>().AsReadOnly()
        };
        private static readonly ConcurrentDictionary<string, ActorBehaviorTreeContainer> MAP = new ConcurrentDictionary<string, ActorBehaviorTreeContainer>();

        private struct ActorBehaviorTreeContainer
        {
            public ActorBehaviorTreeShape Shape { get; }
            public ConcurrentDictionary<long, long> ActorList { get; }

            public ActorBehaviorTreeContainer(
                ActorBehaviorTreeShape shape
            )
            {
                Shape = shape;
                ActorList = new ConcurrentDictionary<long, long>();
            }
            public ActorBehaviorTreeContainer(
                ActorBehaviorTreeShape shape,
                ConcurrentDictionary<long, long> actorList
            )
            {
                Shape = shape;
                ActorList = actorList;
            }

            internal void AddActor(long actorId)
            {
                ActorList.TryAdd(
                    actorId, 
                    actorId
                );
            }

            internal void RemoveActor(long actorId)
            {
                ActorList.TryRemove(
                    actorId, 
                    out _
                );
            }

            internal IEnumerable<long> GetActorList()
            {
                return ActorList.Keys;
            }
        }

        public IEnumerable<long> ActorIdList(
            string treeId
        )
        {
            var container = default(
                ActorBehaviorTreeContainer
            );
            if (!MAP.TryGetValue(
                treeId,
                out container
            ))
            {
                return new List<long>();
            }
            return container.GetActorList();
        }

        public void ClearTrees()
        {
            MAP.Clear();
        }

        public ActorBehaviorTreeShape FindTreeShape(
            string treeId
        )
        {
            var container = default(
                ActorBehaviorTreeContainer
            );
            if (!MAP.TryGetValue(
                treeId,
                out container
            ))
            {
                return DEFAULT_TREE_SHAPE;
            }
            return container.Shape;
        }

        public void RegisterActorToTree(
            long actorId,
            string treeId
        )
        {
            var container = default(
                ActorBehaviorTreeContainer
            );
            if (!MAP.TryGetValue(
                treeId,
                out container
            ))
            {
                throw new ArgumentException(
                    "TreeId not found",
                    "treeId"
                );
            }
            container.AddActor(
                actorId
            );
        }

        public void UnRegisterActorFromTree(long actorId, string treeId)
        {
            var container = default(
                ActorBehaviorTreeContainer
            );
            if (!MAP.TryGetValue(
                treeId,
                out container
            ))
            {
                throw new ArgumentException(
                    "TreeId not found",
                    "treeId"
                );
            }
            container.RemoveActor(
                actorId
            );
        }

        public void RegisterTree(
            string treeId,
            ActorBehaviorTreeShape behaviorTreeShape
        )
        {
            var container = new ActorBehaviorTreeContainer(
                behaviorTreeShape
            );
            if (MAP.TryGetValue(
                treeId,
                out container
            ))
            {
                container = new ActorBehaviorTreeContainer(
                    behaviorTreeShape,
                    container.ActorList
                );
            }
            MAP.AddOrUpdate(
                treeId,
                new ActorBehaviorTreeContainer(
                    behaviorTreeShape
                ),
                (_, __) => container
            );
        }

        public IEnumerable<string> TreeIdList()
        {
            return MAP.Keys;
        }
    }
}