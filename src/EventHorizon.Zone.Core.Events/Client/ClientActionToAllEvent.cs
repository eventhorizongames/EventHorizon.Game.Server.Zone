namespace EventHorizon.Zone.Core.Events.Client
{
    using EventHorizon.Zone.Core.Model.Client;

    public abstract class ClientActionToAllEvent<T> where T : IClientActionData
    {
        public abstract string Action { get; }
        public abstract T Data { get; }

        #region Equals/GetHashCode Generated
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var castObj = obj as ClientActionToAllEvent<T>;

            return Action.Equals(castObj?.Action)
                && Data.Equals(castObj.Data);
        }

        public override int GetHashCode()
        {
            return System.HashCode.Combine(Action, Data);
        }
        #endregion
    }
}
