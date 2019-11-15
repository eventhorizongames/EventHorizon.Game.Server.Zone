using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.DirectoryService;
using EventHorizon.Zone.Core.Events.FileService;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.Core.Reporter.Model;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventHorizon.Zone.Core.Reporter.Save
{
    public class SavePendingReportItemsEventHandler : INotificationHandler<SavePendingReportItemsEvent>
    {
        readonly ILogger _logger;
        readonly IMediator _mediator;
        readonly ServerInfo _serverInfo;
        readonly ReportRepository _repository;

        public SavePendingReportItemsEventHandler(
            ILogger<SavePendingReportItemsEventHandler> logger,
            IMediator mediator,
            ServerInfo serverInfo,
            ReportRepository repository
        )
        {
            _logger = logger;
            _mediator = mediator;
            _serverInfo = serverInfo;
            _repository = repository;
        }

        public async Task Handle(
            SavePendingReportItemsEvent notification,
            CancellationToken cancellationToken
        )
        {
            var reportingPath = GetReportingPath();

            if (!await _mediator.Send(
                new CreateDirectory(
                    reportingPath
                )
            ))
            {
                _logger.LogError(
                    "Failed to create Reporting directory. {ReportingPath}",
                    reportingPath
                );
                return;
            }
            
            var reports = _repository.TakeAll();
            foreach (var report in reports)
            {
                var reportFileFullName = Path.Combine(
                    reportingPath,
                    $"Reporting_{report.Id}.log"
                );
                var reportFileText = "";
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
            }
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
                writer.WriteLine(reportItem.Data);

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