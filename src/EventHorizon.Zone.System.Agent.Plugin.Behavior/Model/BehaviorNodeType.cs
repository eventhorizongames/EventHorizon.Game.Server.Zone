namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Model
{
    public struct BehaviorNodeType
    {
        public static BehaviorNodeType PRIORITY_SELECTOR = new BehaviorNodeType("PRIORITY_SELECTOR");
        public static BehaviorNodeType CONCURRENT_SELECTOR = new BehaviorNodeType("CONCURRENT_SELECTOR");
        public static BehaviorNodeType SEQUENCE_SELECTOR = new BehaviorNodeType("SEQUENCE_SELECTOR");
        public static BehaviorNodeType LOOP_SELECTOR = new BehaviorNodeType("LOOP_SELECTOR");
        public static BehaviorNodeType RANDOM_SELECTOR = new BehaviorNodeType("RANDOM_SELECTOR");
        public static BehaviorNodeType CONDITION = new BehaviorNodeType("CONDITION");
        public static BehaviorNodeType SUB_BEHAVIOR = new BehaviorNodeType("SUB_BEHAVIOR");
        public static BehaviorNodeType ACTION = new BehaviorNodeType("ACTION");
        
        public static BehaviorNodeType Parse(
            string typeAsString
        )
        {
            return new BehaviorNodeType(
                typeAsString
            );
        }

        public string Name { get; }

        private BehaviorNodeType(
            string name
        )
        {
            Name = name;
        }
    }
}