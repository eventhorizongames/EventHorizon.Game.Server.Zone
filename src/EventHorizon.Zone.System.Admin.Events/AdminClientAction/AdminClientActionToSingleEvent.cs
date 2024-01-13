namespace EventHorizon.Zone.System.Admin.AdminClientAction.Client;

using EventHorizon.Zone.System.Admin.AdminClientAction.Model;

public abstract class AdminClientActionToSingleEvent<T>
    where T : IAdminClientActionData
{
    public abstract string ConnectionId { get; }
    public abstract string Action { get; }
    public abstract T Data { get; }

    #region Equals/GetHashCode Generated
    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }


        if (obj is not AdminClientActionToSingleEvent<T> castObj)
        {
            return false;
        }

        return (ConnectionId?.Equals(castObj.ConnectionId) ?? false)
            && (Action?.Equals(castObj.Action) ?? false)
            && (Data?.Equals(castObj.Data) ?? false);
    }

    public override int GetHashCode()
    {
        return global::System.HashCode.Combine(ConnectionId, Action, Data);
    }
    #endregion
}
