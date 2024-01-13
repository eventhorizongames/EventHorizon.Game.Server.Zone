namespace EventHorizon.Zone.System.DataStorage.Model;

using global::System.Diagnostics.CodeAnalysis;

public interface DataStore
{
    bool TryGetValue<T>(
        string key,
        [MaybeNullWhen(false)] out T value
    );

    void AddOrUpdate(
        string key,
        object value
    );
}
