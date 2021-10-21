namespace EventHorizon.Platform.Model
{
    public record PlatformDetailsModel
    {
        public string Version { get; init; } = "0.0.0";

        public PlatformDetailsModel(
            PlatformDetailsOptions options
        )
        {
            Version = options.Version;
        }
    }
}
