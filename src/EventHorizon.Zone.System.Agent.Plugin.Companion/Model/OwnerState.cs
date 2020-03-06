namespace EventHorizon.Zone.System.Agent.Plugin.Companion.Model
{
    public struct OwnerState
    {
        public static readonly string PROPERTY_NAME = "ownerState";

        public string OwnerId { get; set; }
        public bool CanBeCaptured { get; set; }
        public int OwnerFollowDistance { get; set; }

        public object this[string index]
        {
            get
            {
                switch (index)
                {
                    case "ownerId":
                        return this.OwnerId;
                    case "canBeCaptured":
                        return this.CanBeCaptured;
                    case "ownerFollowDistance":
                        return this.OwnerFollowDistance;

                    default:
                        return null;
                }
            }
            set
            {
                switch (index)
                {
                    case "ownerId":
                        this.OwnerId = (string)value;
                        break;
                    case "canBeCaptured":
                        this.CanBeCaptured = (bool)value;
                        break;
                    case "ownerFollowDistance":
                        this.OwnerFollowDistance = (int)value;
                        break;

                    default:
                        break;
                }
            }
        }
    }
}