namespace EventHorizon.Zone.System.ServerModule.State
{
    using EventHorizon.Zone.System.ServerModule.Model;

    using global::System.Collections.Generic;

    public interface ServerModuleRepository
    {
        void Add(
            ServerModuleScripts serverModule
        );
        IEnumerable<ServerModuleScripts> All();
    }
}
