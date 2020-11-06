namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Model
{
    using System;

    public static class BehaviorDefaultSettings
    {
        public static string DEFAULT_SHAPE => "{ \"comments\": \"This is just a default behavior shape that does nothing.\", \"name\": \"DEFAULT\", \"description\": \"\", \"root\": { \"type\": \"ACTION\", \"fire\": \"$DEFAULT$SCRIPT\" } }";
        public static string DEFAULT_SCRIPT => @"
/// <summary>
/// Name: $DEFAULT$SCRIPT.csx
/// </summary>

using EventHorizon.Zone.System.Agent.Plugin.Behavior.Script;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;

return new BehaviorScriptResponse(
    BehaviorNodeStatus.SUCCESS
);
";
    }
}
