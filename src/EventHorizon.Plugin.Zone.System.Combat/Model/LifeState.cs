namespace EventHorizon.Plugin.Zone.System.Combat.Model
{
    public struct LifeState
    {
        public static readonly string PROPERTY_NAME = "LifeState";
        
        public int Health { get; set; }
        public int MaxHealth { get; set; }
        public int Magic { get; set; }
        public int MaxMagic { get; set; }
    }
}