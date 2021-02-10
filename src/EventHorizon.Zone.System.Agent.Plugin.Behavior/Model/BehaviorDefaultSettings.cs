namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Model
{
    using System;

    public static class BehaviorDefaultSettings
    {
        public static string DEFAULT_SHAPE => "{ \"comments\": \"This is just a default behavior shape that does nothing.\", \"name\": \"DEFAULT\", \"description\": \"\", \"root\": { \"type\": \"ACTION\", \"fire\": \"$DEFAULT$SCRIPT\" } }";
    }
}
