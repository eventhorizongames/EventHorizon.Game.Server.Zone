namespace EventHorizon.Platform
{
    public class PlatformDetailsOptions
    {
        public string Version { get; private set; } = "0.0.0";

        public PlatformDetailsOptions SetVersion(
            string version
        )
        {
            Version = version;

            return this;
        }
    }
}
