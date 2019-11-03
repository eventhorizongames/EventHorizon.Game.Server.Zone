using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.Core.Reporter.Model;
using MediatR;

namespace EventHorizon.Zone.Core.Reporter.Save
{
    public struct SavePendingReportItemsEventHandler : INotificationHandler<SavePendingReportItemsEvent>
    {
        readonly ServerInfo _serverInfo;
        readonly ReportRepository _repository;

        public SavePendingReportItemsEventHandler(
            ServerInfo serverInfo,
            ReportRepository repository
        )
        {
            _serverInfo = serverInfo;
            _repository = repository;
        }

        public Task Handle(
            SavePendingReportItemsEvent notification,
            CancellationToken cancellationToken
        )
        {
            var reportingPath = GetReportingPath();
            var reports = _repository.TakeAll();
            if (!Directory.Exists(
                reportingPath
            ))
            {
                new DirectoryInfo(
                    reportingPath
                ).Create();
            }
            foreach (var report in reports)
            {
                var reportFile = Path.Combine(
                    reportingPath,
                    $"Reporting_{report.Id}.log"
                );
                if (File.Exists(
                    reportFile
                ))
                {
                    using (var streamWriter = new FileInfo(
                        reportFile
                    ).AppendText())
                    {
                        AppendReportItemList(
                            report,
                            streamWriter
                        );
                    }
                }
                else
                {
                    using (var streamWriter = new FileInfo(
                        reportFile
                    ).CreateText())
                    {
                        AppendReportItemList(
                            report,
                            streamWriter
                        );
                    }
                }

            }
            return Task.CompletedTask;
        }

        private static void AppendReportItemList(Report report, StreamWriter streamWriter)
        {
            foreach (var reportItem in report.ItemList)
            {
                streamWriter.WriteLine("---");

                streamWriter.WriteLine(reportItem.Message);
                streamWriter.WriteLine(reportItem.Data);

                streamWriter.WriteLine("---");
                streamWriter.WriteLine();
                streamWriter.WriteLine();
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