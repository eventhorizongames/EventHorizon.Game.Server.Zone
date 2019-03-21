namespace EventHorizon.Game.Server.Zone.Agent.Companion.Model
{
    public struct OwnerState
    {
        public static readonly string PROPERTY_NAME = "ownerState";

        public string OwnerId { get; set; }
        public bool CanNotBeCaptured { get; set; }

        public object this[string index]
        {
            get
            {
                switch (index)
                {
                    case "ownerId":
                        return this.OwnerId;
                    case "canNotBeCaptured":
                        return this.CanNotBeCaptured;

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
                    case "canNotBeCaptured":
                        this.CanNotBeCaptured = (bool)value;
                        break;

                    default:
                        break;
                }
            }
        }
    }
}