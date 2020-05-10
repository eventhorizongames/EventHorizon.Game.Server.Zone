namespace EventHorizon.Zone.System.Agent.Model.Path
{
    using global::System.Collections.Generic;
    using global::System.Numerics;

    public struct PathState
    {
        public static readonly string PROPERTY_NAME = "pathState";

        private Queue<Vector3> _path;
        public Vector3 MoveTo { get; set; }

        public Queue<Vector3> Path() => _path;
        public PathState SetPath(
            Queue<Vector3> path
        ) 
        {
            _path = path;
            return this;
        }

        public object this[string index]
        {
            get
            {
                switch (index)
                {
                    case "path":
                        return this._path;
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
                        this._path = (Queue<Vector3>)value;
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
            MoveTo = Vector3.Zero,
        };
    }
}