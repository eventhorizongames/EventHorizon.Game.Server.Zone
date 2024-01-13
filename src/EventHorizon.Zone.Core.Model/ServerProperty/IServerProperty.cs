namespace EventHorizon.Zone.Core.Model.ServerProperty;

public interface IServerProperty
{
    T? Get<T>(
        string key
    );

    void Set(
        string key,
        object value
    );

    void Remove(
        string key
    );
}
