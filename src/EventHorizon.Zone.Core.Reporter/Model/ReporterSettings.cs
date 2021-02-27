namespace EventHorizon.Zone.Core.Reporter.Model
{
    public interface ReporterSettings
    {
        bool IsEnabled { get; }
        bool IsWriteToFileEnabled { get; }
        ElasticsearchReporterSettings Elasticsearch { get; }

        public interface ElasticsearchReporterSettings
        {
            bool IsEnabled { get; }
            string Url { get; }
            string Username { get; }
            string Password { get; }
        }
    }
}