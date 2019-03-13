namespace EventHorizon.Game.Server.Zone.Agent.Companion.Model
{
    public struct OwnerState
    {
        public static readonly string PROPERTY_NAME = "ownerState";
        public string OwnerId { get; set; }
        public string Name { get; set; }
        public object this[string index]
        {
            get
            {
                switch (index)
                {
                    case "ownerId":
                        return this.OwnerId;
                    case "name":
                        return this.Name;

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
                    case "name":
                        this.Name = (string)value;
                        break;

                    default:
                        break;
                }
            }
        }
    }
}