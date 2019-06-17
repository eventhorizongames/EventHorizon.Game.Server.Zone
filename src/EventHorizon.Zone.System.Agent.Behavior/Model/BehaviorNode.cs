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
            SerializedBehaviorNode serailzed
        )
        {
            this.Token = Guid.NewGuid().GetHashCode();
            this.Type = BehaviorNodeType.Parse(
                serailzed.Type
            );
            this.Status = serailzed.Status;
            this.Fire = serailzed.Fire;
            this.FailGate = serailzed.FailGate;
            this.NodeList = new List<BehaviorNode>();
            if (serailzed.NodeList != null)
            {
                this.NodeList = serailzed.NodeList
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