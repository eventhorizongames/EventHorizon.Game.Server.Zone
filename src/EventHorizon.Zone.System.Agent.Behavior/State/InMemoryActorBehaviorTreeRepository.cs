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
            public string TreeId { get; }
            public ActorBehaviorTreeShape Shape { get; }
            public ConcurrentBag<long> ActorList { get; }

            public ActorBehaviorTreeContainer(
                string treeId,
                ActorBehaviorTreeShape shape
            )
            {
                TreeId = treeId;
                Shape = shape;
                ActorList = new ConcurrentBag<long>();
            }
            public ActorBehaviorTreeContainer(
                string treeId,
                ActorBehaviorTreeShape shape,
                ConcurrentBag<long> actorList
            )
            {
                TreeId = treeId;
                Shape = shape;
                ActorList = actorList;
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
            return container.ActorList;
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
            container.ActorList.Add(
                actorId
            );
        }

        public void RegisterTree(
            string treeId,
            ActorBehaviorTreeShape behaviorTreeShape
        )
        {
            var container = new ActorBehaviorTreeContainer(
                treeId,
                behaviorTreeShape
            );
            if (MAP.TryGetValue(
                treeId,
                out container
            ))
            {
                container = new ActorBehaviorTreeContainer(
                    treeId,
                    behaviorTreeShape,
                    container.ActorList
                );
            }
            MAP.AddOrUpdate(
                treeId,
                new ActorBehaviorTreeContainer(
                    treeId,
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