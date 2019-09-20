using EventHorizon.Zone.Core.Model.Client;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Events.Client
{
    public abstract class ClientActionToAllEvent<T> where T : IClientActionData
    {
        public abstract string Action { get; }
        public abstract T Data { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var castObj = obj as ClientActionToAllEvent<T>;

            return Action.Equals(castObj.Action)
                && Data.Equals(castObj.Data);
        }

        public override int GetHashCode()
        {
            var hashCode = 3124515;
            hashCode = hashCode * -12341515 + Action.GetHashCode();
            hashCode = hashCode * -12341515 + Data.GetHashCode();
            return hashCode;
        }
    }
}