using System;
using EventHorizon.Zone.System.Agent.Behavior.Model;
using EventHorizon.Zone.System.Agent.Behavior.State;

namespace EventHorizon.Game.Server.Zone.Tests.Agent.Behavior.TestUtils
{
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

        private static ActorBehaviorTreeShape CreateSingleNodeShape()
        {
            return new ActorBehaviorTreeShape(
                CreateSingleNodeSerializedTree()
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
    }
}