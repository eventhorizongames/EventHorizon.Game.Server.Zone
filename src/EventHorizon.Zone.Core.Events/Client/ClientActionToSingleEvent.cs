namespace EventHorizon.Zone.Core.Events.Client
{
    using EventHorizon.Zone.Core.Model.Client;

    public abstract class ClientActionToSingleEvent<T> where T : IClientActionData
    {
        public abstract string ConnectionId { get; }
        public abstract string Action { get; }
        public abstract T Data { get; }

        #region Equals/GetHashCode Generated
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }


            if (obj is not ClientActionToSingleEvent<T> castObj)
            {
                return false;
            }

            return (ConnectionId?.Equals(castObj.ConnectionId) ?? false)
                && (Action?.Equals(castObj.Action) ?? false)
                && (Data?.Equals(castObj.Data) ?? false);
        }

        public override int GetHashCode()
        {
            return System.HashCode.Combine(ConnectionId, Action, Data);
        }
        #endregion
    }
}
