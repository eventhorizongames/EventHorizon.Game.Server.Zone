namespace EventHorizon.Zone.System.Agent.Plugin.Companion.Model;

// TODO: Move this into a Player Plugin (Player.Plugin.Companion)
public struct CompanionManagementState
{
    public static readonly string PROPERTY_NAME = "companionManagementState";

    public string? CapturedBehaviorTreeId { get; set; }

    public object? this[string index]
    {
        get
        {
            return index switch
            {
                "capturedBehaviorTreeId" => CapturedBehaviorTreeId,
                _ => null,
            };
        }
        set
        {
            switch (index)
            {
                case "capturedBehaviorTreeId":
                    CapturedBehaviorTreeId = (string?)value;
                    break;

                default:
                    break;
            }
        }
    }
}
