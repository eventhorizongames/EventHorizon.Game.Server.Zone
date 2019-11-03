using System;
using System.Collections.Generic;
using System.Linq;

namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Model
{
    public struct BehaviorNode
    {
        public int Token { get; }
        public BehaviorNodeType Type { get; }
        public string Status { get; private set; }
        public IList<BehaviorNode> NodeList { get; private set; }
        public bool IsTraversal { get; }
        public string Fire { get; }
        public int FailGate { get; }
        public bool Reset { get; }

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
                throw new ArgumentException(
                    "BehaviorNode requires a valid SerializedBehaviorNode",
                    "serailzedNode"
                );
            }
            this.Token = Guid.NewGuid().GetHashCode();
            this.Type = BehaviorNodeType.Parse(
                serailzedNode.Type
            );
            this.Status = serailzedNode.Status;

            this.NodeList = new List<BehaviorNode>();
            this.IsTraversal = this.Type.IsTraversal;

            this.Fire = serailzedNode.Fire;
            this.FailGate = serailzedNode.FailGate;
            this.Reset = serailzedNode.Reset;

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


        public override string ToString()
        {
            return $"{Token} : {Type}";
        }
    }
}