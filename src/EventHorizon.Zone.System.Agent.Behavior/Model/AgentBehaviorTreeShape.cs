using System;
using System.Collections.Generic;
using System.Linq;

namespace EventHorizon.Zone.System.Agent.Behavior.Model
{
    public struct AgentBehaviorTreeShape
    {
        public IList<BehaviorNode> NodeList { get; set; }

        public AgentBehaviorTreeShape(
            SerializedAgentBehaviorTree tree
        )
        {
            this.NodeList = new List<BehaviorNode>();
            NodeList.Add(
                new BehaviorNode(
                    tree.Root
                )
            );
            this.FlattenNodeListIntoShape(
                NodeList.First().NodeList
            );
        }

        public BehaviorNode GetNode(
            int nodeToken
        )
        {
            return NodeList.First(a => a.Token == nodeToken).ClearNodeList();
        }

        private void FlattenNodeListIntoShape(
            IList<BehaviorNode> nodeList
        )
        {
            foreach (var node in nodeList)
            {
                NodeList.Add(
                    node
                );
                if (node.NodeList != null)
                {
                    FlattenNodeListIntoShape(
                        node.NodeList
                    );
                }
            }
        }

        public IList<BehaviorNode> GetChildren(
            int nodeToken
        )
        {
            // TODO: Optimize this by making it a dictionary lookup
            return NodeList.First(a => a.Token == nodeToken).NodeList;
        }
    }
}