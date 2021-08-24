namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Model
{
    public struct BehaviorNodeStatus
    {
        public static BehaviorNodeStatus READY = new("READY", null);
        public static BehaviorNodeStatus VISITING = new("VISITING");
        public static BehaviorNodeStatus FAILED = new("FAILED");
        public static BehaviorNodeStatus RUNNING = new("RUNNING");
        public static BehaviorNodeStatus SUCCESS = new("SUCCESS");
        public static BehaviorNodeStatus ERROR = new("ERROR");

        private readonly string _supportedValue;
        private readonly string? _supportedValue2;

        private BehaviorNodeStatus(
            string supportedValue,
            string? supportedValue2
        )
        {
            _supportedValue = supportedValue;
            _supportedValue2 = supportedValue2;
        }

        private BehaviorNodeStatus(
            string supportedValue
        ) : this(supportedValue, supportedValue)
        { }

        public override string ToString()
        {
            return _supportedValue;
        }

        public override bool Equals(
            object? obj
        )
        {
            if (obj == null)
            {
                return EqualTo(
                    null
                );
            }
            else if (obj is string objString)
            {
                return EqualTo(
                    objString
                );
            } 
            else if (obj is BehaviorNodeStatus castedObj)
            {
                return castedObj._supportedValue == _supportedValue
                    || castedObj._supportedValue == _supportedValue2
                    || castedObj._supportedValue2 == _supportedValue
                    || castedObj._supportedValue2 == _supportedValue2;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return global::System.HashCode.Combine(
                _supportedValue,
                _supportedValue2
            );
        }

        private bool EqualTo(
            string? nameAsString
        )
        {
            return _supportedValue == nameAsString
                || _supportedValue2 == nameAsString;
        }
    }
}
