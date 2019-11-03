using System;
using System.Collections.Generic;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.State;

namespace EventHorizon.Game.Server.Zone.Tests.Agent.Behavior.TestUtils
{
    public class BehaviorTreeStateBuilder
    {
        private static IList<SerializedBehaviorNode> EMPTY_LIST = new List<SerializedBehaviorNode>().AsReadOnly();
        
        SerializedBehaviorNode _root;
        List<SerializedBehaviorNode> _nodeList;

        public BehaviorTreeStateBuilder()
        {
            _root = default(SerializedBehaviorNode);
            _nodeList = new List<SerializedBehaviorNode>();
        }

        public BehaviorTreeStateBuilder Root(
            SerializedBehaviorNode root
        )
        {
            _root = root;
            return this;
        }
        public BehaviorTreeStateBuilder AddNode(
            SerializedBehaviorNode node
        )
        {
            node.NodeList = node.NodeList ?? EMPTY_LIST;
            _nodeList.Add(
                node
            );
            return this;
        }

        public BehaviorTreeState Build()
        {
            var shape = BuildShape();
            return new BehaviorTreeState(
                shape
            ).SetShape(
                shape
            );
        }
        public BehaviorTreeState BuildWithLastTraversalStack()
        {
            var shape = BuildShape();
            return new BehaviorTreeState(
                shape
            ).PopActiveNodeFromQueue()
            .PushActiveNodeToTraversalStack()
            .AddActiveTraversalToNextStack()
            .SetShape(
                shape
            );
        }
        private ActorBehaviorTreeShape BuildShape()
        {
            _root.NodeList = _nodeList;
            return new ActorBehaviorTreeShape(
                new SerializedAgentBehaviorTree
                {
                    Root = _root
                }
            );
        }
    }
}