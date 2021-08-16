namespace EventHorizon.Game.Server.Zone.Tests.Agent.Behavior.TestUtils
{
    using System.Collections.Generic;

    using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.State;

    public static class StandardBehaviorTreeState
    {
        public static BehaviorTreeState CreateSingleNode()
        {
            return new BehaviorTreeState(
                CreateSingleNodeShape()
            ).SetShape(
                CreateSingleNodeShape()
            );
        }

        public static BehaviorTreeState CreateNodeWithTraversal()
        {
            var shape = CreateNodeWithTraversalShape();
            return new BehaviorTreeState(
                shape
            ).SetShape(
                shape
            );
        }

        private static ActorBehaviorTreeShape CreateSingleNodeShape()
        {
            return new ActorBehaviorTreeShape(
                "shape",
                CreateSingleNodeSerializedTree()
            );
        }

        private static ActorBehaviorTreeShape CreateNodeWithTraversalShape()
        {
            return new ActorBehaviorTreeShape(
                "shape",
                CreateNodeWithTraversalSerializedTree()
            );
        }

        private static SerializedAgentBehaviorTree CreateSingleNodeSerializedTree()
        {
            return new SerializedAgentBehaviorTree
            {
                Root = new SerializedBehaviorNode
                {
                    Type = "ACTION",
                    Fire = "ACTION_NODE_SCRIPT"
                }
            };
        }

        private static SerializedAgentBehaviorTree CreateNodeWithTraversalSerializedTree()
        {
            return new SerializedAgentBehaviorTree
            {
                Root = new SerializedBehaviorNode
                {
                    Type = "PRIORITY_SELECTOR",
                    NodeList = new List<SerializedBehaviorNode>
                    {
                        new SerializedBehaviorNode
                        {
                            Type = "ACTION",
                            Fire = "ACTION_NODE_SCRIPT",
                        }
                    }
                }
            };
        }
    }
}
