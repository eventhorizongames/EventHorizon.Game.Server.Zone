namespace EventHorizon.Zone.System.Wizard.State;

using EventHorizon.Zone.System.Wizard.Api;
using EventHorizon.Zone.System.Wizard.Model;

using global::System.Collections.Concurrent;
using global::System.Collections.Generic;

public class StandardWizardRepository
    : WizardRepository
{
    public ConcurrentDictionary<string, WizardMetadata> _map = new();

    public IEnumerable<WizardMetadata> All => _map.Values;

    public void Clear() => _map.Clear();

    public Option<WizardMetadata> Get(
        string id
    )
    {
        if (_map.TryGetValue(
            id,
            out var value
        ))
        {
            return value
                .ToOption();
        }

        return new(
            null
        );
    }

    public void Set(
        string id,
        WizardMetadata wizard
    ) => _map.AddOrUpdate(
        id,
        wizard,
        (_, _) => wizard
    );
}
