using System;
using System.Linq;
using System.Collections.Generic;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;
using EventHorizon.Zone.Core.Reporter.Model;

namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.State
{
    public partial struct BehaviorTreeState
    {
        private bool _checkTraversal;
        private int _activeNodeToken;
        private int _activeTraversalToken;
        private ActorBehaviorTreeShape _shape;
        
        private string _reportId;
        private ReportTracker _reportTracker;

        public bool ContainsNext => TraversalStack.Count > 0;
        public bool CheckTraversal => _checkTraversal;

        public BehaviorNode ActiveNode => GetNode(
            _activeNodeToken
        );

        public BehaviorNode ActiveTraversal => GetNode(
            _activeTraversalToken
        );
        public IDictionary<int, BehaviorNode> NodeMap { get; private set; }
        public IList<int> TraversalStack { get; private set; }
        public IList<int> LastTraversalStack { get; private set; }
        public IList<int> NextTraversalStack { get; }

        public BehaviorTreeState(
            ActorBehaviorTreeShape shape
        )
        {
            _shape = shape;
            _checkTraversal = false;
            _activeNodeToken = -1;
            _activeTraversalToken = -1;
            _reportId = null;
            _reportTracker = null;
            ShapeQueue = new Queue<int>();

            NodeMap = new Dictionary<int, BehaviorNode>();
            TraversalStack = new List<int>();
            LastTraversalStack = new List<int>();
            NextTraversalStack = new List<int>();

            this.SetupQueueFromShape(
                shape
            );
        }
        public bool IsValid => _shape.IsValid;
        public Queue<int> ShapeQueue { get; private set; }

        public bool IsActiveNodeValidAndNotRunning()
        {
            return ActiveNode.Token != 0
                && !BehaviorNodeStatus.RUNNING.Equals(
                    ActiveNode.Status
                );
        }

        public BehaviorTreeState SetShape(
            ActorBehaviorTreeShape shape
        )
        {
            _shape = shape;
            _checkTraversal = false;
            this.SetupQueueFromShape(
                shape
            );
            // This will keep only the running, not traversal, Nodes in state.
            NodeMap = NodeMap.Where(
                a => BehaviorNodeStatus.RUNNING.Equals(
                    a.Value.Status
                ) && !a.Value.IsTraversal
            ).ToDictionary(
                node => node.Key,
                node => node.Value
            );
            // This set the last traversal State into the current for this state.
            LastTraversalStack = NextTraversalStack.Reverse().ToList();
            // Clear the next traversal state to be rebuilt next run.
            NextTraversalStack.Clear();
            // Clear the traversal stack
            TraversalStack.Clear();

            return this;
        }

        public BehaviorTreeState PopActiveNodeFromQueue()
        {
            this.Report(
                "PopActiveNodeFromQueue ENTER",
                new { _activeNodeToken = _activeNodeToken }
            );
            if (ShapeQueue.Count != 0)
            {
                this._activeNodeToken = ShapeQueue.Dequeue();
            }
            else
            {
                this._activeNodeToken = -1;
            }
            this.Report(
                "PopActiveNodeFromQueue EXIT",
                new { _activeNodeToken = _activeNodeToken }
            );
            return this;
        }

        public BehaviorTreeState SetCheckTraversal(
            bool checkTraversal
        )
        {
            _checkTraversal = checkTraversal;
            return this;
        }

        public BehaviorTreeState AdvanceQueueToAfterPassedToken(
            int token
        )
        {
            var result = this.Report(
                "AdvanceQueueToAfterPassedToken ENTER",
                new { token }
            );
            // Move the queue to the current token location
            while (result._activeNodeToken != token && result._activeNodeToken != -1)
            {
                result = result.PopActiveNodeFromQueue();
            }
            result = result.Report(
                "AdvanceQueueToAfterPassedToken EXIT",
                new { token, result._activeNodeToken }
            );
            // Pop out the current token to set next node token
            return result;
        }

        public bool ContainedInLastTraversal(
            int token
        )
        {
            return LastTraversalStack.Contains(
                token
            );
        }
        public BehaviorTreeState RemoveNodeFromLastTraversalStack(
            int token
        )
        {
            LastTraversalStack.Remove(
                token
            );
            return this;
        }

        private void SetupQueueFromShape(
            ActorBehaviorTreeShape shape
        )
        {
            ShapeQueue.Clear();
            foreach (var node in shape.NodeList)
            {
                ShapeQueue.Enqueue(
                    node.Token
                );
            }
        }
    }
}