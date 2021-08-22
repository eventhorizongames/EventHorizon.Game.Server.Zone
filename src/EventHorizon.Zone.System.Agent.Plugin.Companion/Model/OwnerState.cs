namespace EventHorizon.Zone.System.Agent.Plugin.Companion.Model
{
    public struct OwnerState
    {
        public static readonly string PROPERTY_NAME = "ownerState";

        public string OwnerId { get; set; }
        public bool CanBeCaptured { get; set; }
        public int OwnerFollowDistance { get; set; }

        public object? this[string index]
        {
            get
            {
                return index switch
                {
                    "ownerId" => OwnerId,
                    "canBeCaptured" => CanBeCaptured,
                    "ownerFollowDistance" => OwnerFollowDistance,
                    _ => null,
                };
            }
            set
            {
                switch (index)
                {
                    case "ownerId":
                        OwnerId = (string?)value ?? string.Empty;
                        break;
                    case "canBeCaptured":
                        CanBeCaptured = (bool?)value ?? false;
                        break;
                    case "ownerFollowDistance":
                        OwnerFollowDistance = (int?)value ?? 1;
                        break;

                    default:
                        break;
                }
            }
        }
    }
}
