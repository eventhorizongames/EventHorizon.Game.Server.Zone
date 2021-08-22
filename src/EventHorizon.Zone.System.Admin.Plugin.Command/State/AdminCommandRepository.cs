namespace EventHorizon.Zone.System.Admin.Plugin.Command.State
{
    using global::System.Collections.Generic;

    using EventHorizon.Zone.System.Admin.Plugin.Command.Model;

    public interface AdminCommandRepository
    {
        void Clear();
        void Add(
            AdminCommandInstance adminCommandInstance
        );
        IEnumerable<AdminCommandInstance> Where(
            string command
        );
    }
}
