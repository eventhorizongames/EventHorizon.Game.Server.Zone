namespace EventHorizon.Zone.Core.Model.Entity.Movement
{
    public struct MovementState
    {
        public static readonly string PROPERTY_NAME = "movementState";

        public float Speed { get; set; }

        public object this[string index]
        {
            get
            {
                switch (index)
                {
                    case "speed":
                        return this.Speed;

                    default:
                        return null;
                }
            }
            set
            {
                switch (index)
                {
                    case "speed":
                        this.Speed = (float)value;
                        break;

                    default:
                        break;
                }
            }
        }

        public static readonly MovementState NEW = new MovementState
        {
            Speed = 1.0f,
        };
    }
}
