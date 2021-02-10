namespace EventHorizon.Zone.System.Server.Scripts.Plugin.Compiler.Api
{
    using global::System.Reflection;
    using global::System.Threading.Tasks;

    public interface AssemblyBuilder
    {
        void ReferenceAssembly(
            Assembly assembly
        );
        Task<string> Compile(
            string consolidatedClasses
        );
    }
}