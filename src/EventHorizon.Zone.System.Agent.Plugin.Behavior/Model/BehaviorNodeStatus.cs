using System.Collections.Generic;
using System.Linq;

namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Model
{
    public struct BehaviorNodeStatus
    {
        public static BehaviorNodeStatus READY = new BehaviorNodeStatus("READY", null);
        public static BehaviorNodeStatus VISITING = new BehaviorNodeStatus("VISITING");
        public static BehaviorNodeStatus FAILED = new BehaviorNodeStatus("FAILED");
        public static BehaviorNodeStatus RUNNING = new BehaviorNodeStatus("RUNNING");
        public static BehaviorNodeStatus SUCCESS = new BehaviorNodeStatus("SUCCESS");
        public static BehaviorNodeStatus ERROR = new BehaviorNodeStatus("ERROR");

        private IList<string> _supportedValues;

        private BehaviorNodeStatus(
            string supportedValue,
            string supportedValue2
        )
        {
            this._supportedValues = new List<string>() { supportedValue };
            if (supportedValue != supportedValue2)
            {
                this._supportedValues.Add(
                    supportedValue2
                );
            }
        }

        private BehaviorNodeStatus(
            string supportedValue
        ) : this(supportedValue, supportedValue)
        { }

        public override string ToString()
        {
            return this._supportedValues.First();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return EqualTo(
                    null
                );
            }
            if (obj.GetType() == typeof(string))
            {
                return EqualTo(
                    (string)obj
                );
            }
            if (GetType() != obj.GetType())
            {
                return false;
            }

            var castedObj = (BehaviorNodeStatus)obj;
            if (castedObj._supportedValues == null || castedObj._supportedValues == null)
            {
                return false;
            }

            return this._supportedValues
                .Where(
                    value => castedObj._supportedValues.Contains(
                        value
                    )
                ).Count() > 0;
        }

        public override int GetHashCode()
        {
            return this._supportedValues.GetHashCode();
        }

        private bool EqualTo(
            string nameAsString
        )
        {
            return this._supportedValues.Contains(
                nameAsString
            );
        }
    }
}
