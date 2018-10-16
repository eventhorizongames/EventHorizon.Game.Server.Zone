namespace EventHorizon.Plugin.Zone.System.Combat.Model.Life
{
    public struct LifeCondition
    {
        public static LifeCondition NULL = default(LifeCondition);
        public static LifeCondition ALIVE = new LifeCondition("ALIVE");
        public static LifeCondition DEAD = new LifeCondition("DEAD");
        
        public string Name { get; }

        public LifeCondition(string name)
        {
            Name = name;
        }
    }
}