namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;

using global::System;
using global::System.Collections.Generic;
using global::System.Linq;

public struct BehaviorNode
{
    public int Token { get; }
    public BehaviorNodeType Type { get; }
    public string? Status { get; private set; }
    public IList<BehaviorNode>? NodeList { get; private set; }
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
        int tokenRoot,
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

        Token = serailzedNode.GetToken(
            tokenRoot
        );
        Type = BehaviorNodeType.Parse(
            serailzedNode.Type
        );
        Status = serailzedNode.Status;

        NodeList = new List<BehaviorNode>();
        IsTraversal = Type.IsTraversal;

        Fire = serailzedNode.Fire;
        FailGate = serailzedNode.FailGate;
        Reset = serailzedNode.Reset;

        if (serailzedNode.NodeList != null)
        {
            var childTokenRoot = tokenRoot;
            foreach (var childNode in serailzedNode.NodeList)
            {
                childTokenRoot++;
                NodeList.Add(
                    new BehaviorNode(
                        childTokenRoot,
                        childNode
                    )
                );
            }
        }
    }

    public BehaviorNode UpdateStatus(
        string status
    )
    {
        Status = status;
        return this;
    }


    public override string ToString()
    {
        return $"{Token} : {Type}";
    }
}
