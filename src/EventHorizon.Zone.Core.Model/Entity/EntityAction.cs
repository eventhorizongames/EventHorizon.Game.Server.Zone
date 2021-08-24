namespace EventHorizon.Zone.Core.Model.Entity
{
    public class EntityAction
    {
        public static readonly EntityAction POSITION = new("Entity.Position");
        public static readonly EntityAction ADD = new("Entity.Add");
        public static readonly EntityAction REMOVE = new("Entity.Remove");
        public static readonly EntityAction PROPERTY_CHANGED = new("Entity.PropertyChanged");

        public string Type { get; }
        protected EntityAction(string type)
        {
            Type = type;
        }

        /// <summary>
        /// Validate that an Entity Action matches.
        /// Can also pass in String representation of Type.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }


            return Type.Equals(((EntityAction)obj).Type);
        }

        /// <summary>
        /// Returns hash code for Entity Action.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Type.GetHashCode();
        }
    }
}
