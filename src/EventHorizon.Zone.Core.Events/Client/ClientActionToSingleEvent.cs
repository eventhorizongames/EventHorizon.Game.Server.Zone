using EventHorizon.Zone.Core.Model.Client;

namespace EventHorizon.Zone.Core.Events.Client
{
    public abstract class ClientActionToSingleEvent<T> where T : IClientActionData
    {
        public abstract string ConnectionId { get; }
        public abstract string Action { get; }
        public abstract T Data { get; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var castObj = obj as ClientActionToSingleEvent<T>;

            return (ConnectionId?.Equals(castObj.ConnectionId) ?? false)
                && (Action?.Equals(castObj.Action) ?? false)
                && (Data?.Equals(castObj.Data) ?? false);
        }

        public override int GetHashCode()
        {
            var hashCode = 3124515;
            hashCode = hashCode * -12341515 + ConnectionId.GetHashCode();
            hashCode = hashCode * -12341515 + Action.GetHashCode();
            hashCode = hashCode * -12341515 + Data.GetHashCode();
            return hashCode;
        }
    }
}
