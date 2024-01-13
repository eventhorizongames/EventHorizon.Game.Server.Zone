namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;

using global::System.Collections.Generic;

public struct BehaviorNodeType
{
    public static IList<string> TRAVERSAL_NODE_LIST = new List<string>
    {
        "PRIORITY_SELECTOR",
        "CONCURRENT_SELECTOR",
        "SEQUENCE_SELECTOR",
        "LOOP_SELECTOR",
        "RANDOM_SELECTOR",
        "SUB_BEHAVIOR",
    }.AsReadOnly();
    public static BehaviorNodeType PRIORITY_SELECTOR = new("PRIORITY_SELECTOR", true);
    public static BehaviorNodeType CONCURRENT_SELECTOR = new("CONCURRENT_SELECTOR", true);
    public static BehaviorNodeType SEQUENCE_SELECTOR = new("SEQUENCE_SELECTOR", true);
    public static BehaviorNodeType LOOP_SELECTOR = new("LOOP_SELECTOR", true);
    public static BehaviorNodeType RANDOM_SELECTOR = new("RANDOM_SELECTOR", true);
    public static BehaviorNodeType CONDITION = new("CONDITION", false);
    public static BehaviorNodeType SUB_BEHAVIOR = new("SUB_BEHAVIOR", true);
    public static BehaviorNodeType ACTION = new("ACTION", false);

    public static BehaviorNodeType Parse(
        string typeAsString
    )
    {
        return new BehaviorNodeType(
            typeAsString,
            TRAVERSAL_NODE_LIST.Contains(
                typeAsString
            )
        );
    }

    public string Name { get; }
    public bool IsTraversal { get; }

    private BehaviorNodeType(
        string name,
        bool isTraversal
    )
    {
        Name = name;
        IsTraversal = isTraversal;
    }

    public override string ToString()
    {
        return Name;
    }
}
