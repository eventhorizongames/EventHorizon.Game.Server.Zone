namespace EventHorizon.Zone.System.Admin.AdminClientAction.Client
{
    using EventHorizon.Zone.System.Admin.AdminClientAction.Model;

    public abstract class AdminClientActionToAllEvent<T>
        where T : IAdminClientActionData
    {
        public abstract string Action { get; }
        public abstract T Data { get; }

        #region Equals/GetHashCode Generated
        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var castObj = obj as AdminClientActionToAllEvent<T>;

            return Action.Equals(castObj?.Action)
                && Data.Equals(castObj.Data);
        }

        public override int GetHashCode()
        {
            return global::System.HashCode.Combine(Action, Data);
        }
        #endregion
    }
}
