using System.Collections.Generic;
using System.Linq;

namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Model
{
    public struct ActorBehaviorTreeShape
    {
        private static IList<BehaviorNode> EMPTY_LIST = new List<BehaviorNode>().AsReadOnly();

        public IList<BehaviorNode> NodeList { get; set; }
        public bool IsValid => NodeList != null;

        public ActorBehaviorTreeShape(
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
            return NodeList.First(
                node => node.Token == nodeToken
            ).ClearNodeList();
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
            // PERF: Optimize this by making it a dictionary lookup
            return NodeList?.FirstOrDefault(
                node => node.Token == nodeToken
            ).NodeList ?? EMPTY_LIST;
        }
    }
}