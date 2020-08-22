namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.State
{
    using global::System;
    using global::System.Linq;
    using global::System.Collections.Generic;
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;
    using EventHorizon.Zone.Core.Reporter.Model;

    public partial struct BehaviorTreeState
    {
        public static readonly string PROPERTY_NAME = "BehaviorTreeState";

        private bool _checkTraversal;
        private int _activeNodeToken;
        private int _activeTraversalToken;
        private ActorBehaviorTreeShape _shape;

        private string _reportId;
        private string _reportCorrelationId;
        private ReportTracker _reportTracker;

        public bool ContainsNext => TraversalStack.Count > 0;
        public bool CheckTraversal => _checkTraversal;

        public BehaviorNode ActiveNode => GetNode(
            _activeNodeToken
        );

        public BehaviorNode ActiveTraversal => GetNode(
            _activeTraversalToken
        );

        public string ShapeId => _shape.Id;

        private IDictionary<int, BehaviorNode> NodeMap { get; set; }
        private IList<int> TraversalStack { get; set; }
        private IList<int> LastTraversalStack { get; set; }
        private IList<int> NextTraversalStack { get; set; }

        public IEnumerable<BehaviorNode> NodeList() => NodeMap.Values;

        public BehaviorTreeState(
            ActorBehaviorTreeShape shape
        )
        {
            _shape = shape;
            _checkTraversal = false;
            _activeNodeToken = -1;
            _activeTraversalToken = -1;
            _reportId = null;
            _reportCorrelationId = null;
            _reportTracker = null;
            ShapeQueue = new Queue<int>();
            ShapeOrder = new int[0];

            NodeMap = new Dictionary<int, BehaviorNode>();
            TraversalStack = new List<int>();
            LastTraversalStack = new List<int>();
            NextTraversalStack = new List<int>();

            SetupQueueFromShape(
                shape
            );
        }
        public bool IsValid => _shape.IsValid;
        private Queue<int> ShapeQueue { get; set; }
        private int[] ShapeOrder { get; set; }

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
            NextTraversalStack = new List<int>();
            // Clear the traversal stack
            TraversalStack = new List<int>();

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

        public BehaviorTreeState AdvanceQueueToPassedToken(
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
            if (!shape.IsValid)
            {
                return;
            }
            ShapeOrder = new int[shape.NodeList.Count];
            var index = 0;
            foreach (var node in shape.NodeList)
            {
                ShapeQueue.Enqueue(
                    node.Token
                );
                ShapeOrder[index] = node.Token;
                index++;
            }
        }
    }
}