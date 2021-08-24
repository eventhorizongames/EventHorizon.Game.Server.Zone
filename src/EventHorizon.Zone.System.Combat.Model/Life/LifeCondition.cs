namespace EventHorizon.Zone.System.Combat.Model.Life
{
    public struct LifeCondition
    {
        public static LifeCondition NULL = default;
        public static LifeCondition ALIVE = new("ALIVE");
        public static LifeCondition DEAD = new("DEAD");

        public string Name { get; }

        public LifeCondition(string name)
        {
            Name = name;
        }
    }
}
