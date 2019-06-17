using System.Linq;
using System.Collections.Generic;
using EventHorizon.Zone.System.Agent.Behavior.Model;

namespace EventHorizon.Zone.System.Agent.Behavior.State
{
    public partial struct BehaviorTreeState
    {
        private int _activeNodeToken;
        private int _activeTraversalToken;
        private AgentBehaviorTreeShape _shape;

        public bool ContainsNext => TraversalStack.Count > 0;

        public BehaviorNode ActiveNode => GetNode(
            _activeNodeToken
        );

        public BehaviorNode ActiveTraversal => GetNode(
            _activeTraversalToken
        );
        public IDictionary<int, BehaviorNode> NodeMap { get; }
        public IList<int> TraversalStack { get; }
        public IList<int> NextTraversalStack { get; }

        public BehaviorTreeState(
            AgentBehaviorTreeShape shape
        )
        {
            _shape = shape;
            _activeNodeToken = shape.NodeList.First().Token;
            _activeTraversalToken = 0;

            NodeMap = new Dictionary<int, BehaviorNode>();
            TraversalStack = new List<int>();
            NextTraversalStack = new List<int>();
        }
    }
}