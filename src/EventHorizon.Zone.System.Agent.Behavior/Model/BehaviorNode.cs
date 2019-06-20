using System;
using System.Collections.Generic;
using System.Linq;

namespace EventHorizon.Zone.System.Agent.Behavior.Model
{
    public struct BehaviorNode
    {
        public int Token { get; }
        public BehaviorNodeType Type { get; }
        public string Status { get; private set; }
        public string Fire { get; }
        public int FailGate { get; }
        public IList<BehaviorNode> NodeList { get; private set; }

        public BehaviorNode ClearNodeList()
        {
            NodeList = null;
            return this;
        }

        public BehaviorNode(
            SerializedBehaviorNode serailzedNode
        )
        {
            if (serailzedNode == null)
            {
                throw new ArgumentException("BehaviorNode requires a valid SerializedBehaviorNode", "serailzedNode");
            }
            this.Token = Guid.NewGuid().GetHashCode();
            this.Type = BehaviorNodeType.Parse(
                serailzedNode.Type
            );
            this.Status = serailzedNode.Status;
            this.Fire = serailzedNode.Fire;
            this.FailGate = serailzedNode.FailGate;
            this.NodeList = new List<BehaviorNode>();
            if (serailzedNode.NodeList != null)
            {
                this.NodeList = serailzedNode.NodeList
                    .Select(
                        node => new BehaviorNode(
                            node
                        )
                    ).ToList();
            }
        }

        public BehaviorNode UpdateStatus(
            string status
        )
        {
            this.Status = status;
            return this;
        }

        // public bool IsTraversalNode()
        // {
        //     if (BehaviorNodeType.PRIORITY_SELECTOR.Equals(Type)
        //         || BehaviorNodeType.CONCURRENT_SELECTOR.Equals(Type)
        //         || BehaviorNodeType.SEQUENCE_SELECTOR.Equals(Type)
        //         || BehaviorNodeType.LOOP_SELECTOR.Equals(Type)
        //         || BehaviorNodeType.RANDOM_SELECTOR.Equals(Type))
        //     {
        //         return true;
        //     }
        //     return false;
        // }
    }
}