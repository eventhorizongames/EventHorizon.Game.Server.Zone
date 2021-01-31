namespace EventHorizon.Zone.Core.Reporter.Writer
{
    using System.IO;
    using System.Linq;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using EventHorizon.Zone.Core.Events.DirectoryService;
    using EventHorizon.Zone.Core.Events.FileService;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.Core.Reporter.Model;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public class WriteReportToFileHandler
        : IRequestHandler<WriteReportToFile>
    {
        private static readonly JsonSerializerOptions JSON_OPTIONS = new JsonSerializerOptions
        {
            WriteIndented = true,
        };

        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly ServerInfo _serverInfo;

        public WriteReportToFileHandler(
            ILogger<WriteReportToFileHandler> logger,
            IMediator mediator,
            ServerInfo serverInfo
        )
        {
            _logger = logger;
            _mediator = mediator;
            _serverInfo = serverInfo;
        }

        public async Task<Unit> Handle(
            WriteReportToFile request,
            CancellationToken cancellationToken
        )
        {
            var report = request.Report;
            var reportingPath = GetReportingPath();
            var reportFileFullName = Path.Combine(
                reportingPath,
                $"Reporting_{report.Id}.log"
            );
            var reportFileText = "";
            if (!await _mediator.Send(
                new CreateDirectory(
                    reportingPath
                )
            ))
            {
                _logger.LogError(
                    "Failed to create Reporting directory. {ReportingPath} {Report}",
                    reportingPath,
                    report
                );
                return Unit.Value;
            }

            if (!report.ItemList?.Any() ?? true)
            {
                return Unit.Value;
            }

            using (var streamWriter = new StringWriter())
            {
                AppendReportItemList(
                    report,
                    streamWriter
                );
                reportFileText = streamWriter.ToString();
            }
            await _mediator.Send(
                new AppendTextToFile(
                    reportFileFullName,
                    reportFileText
                )
            );

            return Unit.Value;
        }

        private static void AppendReportItemList(
            Report report,
            StringWriter writer
        )
        {
            foreach (var reportItem in report.ItemList)
            {
                writer.WriteLine("---");

                writer.WriteLine(reportItem.Message);
                if (reportItem.Data != null)
                {
                    writer.WriteLine(
                        JsonSerializer.Serialize(
                            reportItem.Data,
                            JSON_OPTIONS
                        )
                    );
                }

                writer.WriteLine("---");
                writer.WriteLine();
                writer.WriteLine();
            }
        }

        private string GetReportingPath()
        {
            return Path.Combine(
                _serverInfo.AppDataPath,
                "Reporting"
            );
        }
    }
}
