using System.Collections.Generic;

using EventHorizon.Zone.System.EntityModule.Model;

namespace EventHorizon.Zone.System.EntityModule.Api
{
    public interface EntityModuleRepository
    {
        void AddBaseModule(EntityScriptModule baseModule);
        void AddPlayerModule(EntityScriptModule playerModule);
        IEnumerable<EntityScriptModule> ListOfAllBaseModules();
        IEnumerable<EntityScriptModule> ListOfAllPlayerModules();
    }
}
