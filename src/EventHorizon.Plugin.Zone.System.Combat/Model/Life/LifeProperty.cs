namespace EventHorizon.Plugin.Zone.System.Combat.Model.Life
{
    public struct LifeProperty
    {
        public static readonly LifeProperty HP = new LifeProperty("HP");
        public static readonly LifeProperty AP = new LifeProperty("AP");

        public string Property { get; }

        public LifeProperty(string property)
        {
            Property = property;
        }
    }
}