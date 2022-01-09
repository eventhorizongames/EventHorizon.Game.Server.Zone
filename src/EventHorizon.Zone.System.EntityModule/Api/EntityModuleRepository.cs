namespace EventHorizon.Zone.System.EntityModule.Api;

using global::System.Collections.Generic;

using EventHorizon.Zone.System.EntityModule.Model;

public interface EntityModuleRepository
{
    void Clear();
    void AddBaseModule(
        EntityScriptModule baseModule
    );
    void AddPlayerModule(
        EntityScriptModule playerModule
    );
    IEnumerable<EntityScriptModule> ListOfAllBaseModules();
    IEnumerable<EntityScriptModule> ListOfAllPlayerModules();
}
