namespace EventHorizon.Zone.System.Agent.Model.Path
{
    using global::System.Collections.Generic;
    using global::System.Numerics;

    public struct PathState
    {
        public static readonly string PROPERTY_NAME = "pathState";

        public Queue<Vector3> Path { get; set; }
        public Vector3 MoveTo { get; set; }

        public object this[string index]
        {
            get
            {
                switch (index)
                {
                    case "path":
                        return this.Path;
                    case "moveTo":
                        return this.MoveTo;

                    default:
                        return null;
                }
            }
            set
            {
                switch (index)
                {
                    case "path":
                        this.Path = (Queue<Vector3>)value;
                        break;
                    case "moveTo":
                        this.MoveTo = (Vector3)value;
                        break;

                    default:
                        break;
                }
            }
        }

        public static readonly PathState NEW = new PathState
        {
            Path = null,
            MoveTo = Vector3.Zero,
        };
    }
}